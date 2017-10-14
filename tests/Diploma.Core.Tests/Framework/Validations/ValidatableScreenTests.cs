using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Diploma.Core.Framework.Validations;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace Diploma.Core.Tests.Framework.Validations
{
    [TestFixture]
    [SuppressMessage("ReSharper", "AsyncConverter.ConfigureAwaitHighlighting", Justification = "This is tests")]
    public class ValidatableScreenTests
    {
        private MyValidatableScreen _myValidatableScreen;

        private Mock<IValidationAdapter> _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new Mock<IValidationAdapter>();
            _myValidatableScreen = new MyValidatableScreen(_validator.Object);
        }

        [Test]
        public void Event_Raised_And_HasErrors_Changed_If_Error_Was_Empty_Array_And_Now_Is_Not()
        {
            _validator.Setup(x => x.ValidatePropertyAsync("IntProperty"))
                .ReturnsAsync(new string[0]);

            var unused1 = _myValidatableScreen.ValidateProperty("IntProperty");

            string changedProperty = null;
            _myValidatableScreen.ErrorsChanged += (o, e) => changedProperty = e.PropertyName;
            var hasErrorsRaised = false;
            _myValidatableScreen.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == "HasErrors")
                {
                    hasErrorsRaised = true;
                }
            };

            _validator.Setup(x => x.ValidatePropertyAsync("IntProperty"))
                .ReturnsAsync(new[] { "error" });

            var unused2 = _myValidatableScreen.ValidateProperty("IntProperty");

            changedProperty.ShouldBe("IntProperty");
            hasErrorsRaised.ShouldBeTrue();
        }

        [Test]
        public void Event_Raised_And_HasErrors_Changed_If_Error_Was_Null_And_Now_Is_Not()
        {
            _validator.Setup(x => x.ValidatePropertyAsync("IntProperty"))
                .ReturnsAsync(() => null);

            var unused1 = _myValidatableScreen.ValidateProperty("IntProperty");

            string changedProperty = null;
            _myValidatableScreen.ErrorsChanged += (o, e) => changedProperty = e.PropertyName;
            var hasErrorsRaised = false;
            _myValidatableScreen.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == "HasErrors")
                {
                    hasErrorsRaised = true;
                }
            };

            _validator.Setup(x => x.ValidatePropertyAsync("IntProperty"))
                .ReturnsAsync(new[] { "error" });

            var unused2 = _myValidatableScreen.ValidateProperty("IntProperty");

            changedProperty.ShouldBe("IntProperty");
            hasErrorsRaised.ShouldBeTrue();
        }

        [Test]
        public void Event_Raised_And_HasErrors_Changed_If_Error_Was_Set_And_Is_Now_Empty_Array()
        {
            _validator.Setup(x => x.ValidatePropertyAsync("IntProperty"))
                .ReturnsAsync(new[] { "error" });

            var unused1 = _myValidatableScreen.ValidateProperty("IntProperty");

            string changedProperty = null;
            _myValidatableScreen.ErrorsChanged += (o, e) => changedProperty = e.PropertyName;
            var hasErrorsRaised = false;
            _myValidatableScreen.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == "HasErrors")
                {
                    hasErrorsRaised = true;
                }
            };

            _validator.Setup(x => x.ValidatePropertyAsync("IntProperty"))
                .ReturnsAsync(new string[0]);

            var unused2 = _myValidatableScreen.ValidateProperty("IntProperty");
            
            changedProperty.ShouldBe("IntProperty");
            hasErrorsRaised.ShouldBeTrue();
        }

        [Test]
        public void Event_Raised_And_HasErrors_Changed_If_Error_Was_Set_And_Is_Now_Null()
        {
            _validator.Setup(x => x.ValidatePropertyAsync("IntProperty"))
                .ReturnsAsync(new[] { "error" });

            var unused1 = _myValidatableScreen.ValidateProperty("IntProperty");

            string changedProperty = null;
            _myValidatableScreen.ErrorsChanged += (o, e) => changedProperty = e.PropertyName;
            var hasErrorsRaised = false;
            _myValidatableScreen.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == "HasErrors")
                {
                    hasErrorsRaised = true;
                }
            };

            _validator.Setup(x => x.ValidatePropertyAsync("IntProperty"))
                .ReturnsAsync(() => null);

            var unused2 = _myValidatableScreen.ValidateProperty("IntProperty");

            changedProperty.ShouldBe("IntProperty");
            hasErrorsRaised.ShouldBeTrue();
        }

        [Test]
        public void Event_Raised_And_HasErrors_Changed_If_Validate_All_And_Errors_Change()
        {
            _validator.Setup(x => x.ValidateAllPropertiesAsync())
                .ReturnsAsync(
                    new Dictionary<string, string[]>
                    {
                        { "IntProperty", new[] { "error" } },
                        { "OtherProperty", null },
                        { "OtherOtherProperty", new string[0] },
                        { "PropertyThatWillDisappear", new[] { "error" } }
                    });

            var unused1 = _myValidatableScreen.Validate();

            _validator.Setup(x => x.ValidateAllPropertiesAsync())
                .ReturnsAsync(
                    new Dictionary<string, string[]>
                    {
                        { "IntProperty", new[] { "error" } },
                        { "OtherProperty", new[] { "error" } },
                        { "OtherOtherProperty", new string[0] },
                        { "NewOKProperty", null },
                        { "NewNotOKProperty", new[] { "woo" } }
                    });

            var errors = new List<string>();
            _myValidatableScreen.ErrorsChanged += (o, e) => errors.Add(e.PropertyName);
            var hasErrorsChangedCount = 0;
            _myValidatableScreen.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == "HasErrors")
                {
                    hasErrorsChangedCount++;
                }
            };

            var unused2 = _myValidatableScreen.Validate();
            
            errors.ShouldBe(new[] { "OtherProperty", "NewOKProperty", "NewNotOKProperty", "PropertyThatWillDisappear" });
            hasErrorsChangedCount.ShouldBe(1);
        }

        [Test]
        public void GetErrors_Returns_Errors_For_Property()
        {
            _validator.Setup(x => x.ValidatePropertyAsync("IntProperty"))
                .ReturnsAsync(new[] { "error1", "error2" });

            var unused = _myValidatableScreen.ValidateProperty("IntProperty");

            var errors = _myValidatableScreen.GetErrors("IntProperty");

            errors.ShouldBe(new[] { "error1", "error2" });
        }

        [Test]
        public void GetErrors_Returns_Null_If_No_Errors_For_That_Property()
        {
            var errors = _myValidatableScreen.GetErrors("FooBar");
            errors.ShouldBeNull();
        }

        [Test]
        public void GetErrors_With_Null_Returns_Model_Errors()
        {
            _validator.Setup(x => x.ValidateAllPropertiesAsync())
                .ReturnsAsync(
                    new Dictionary<string, string[]>
                    {
                        { string.Empty, new[] { "error1", "error2" } }
                    });

            var unused = _myValidatableScreen.Validate();

            var errors = _myValidatableScreen.GetErrors(null);
            errors.ShouldBe(new[] { "error1", "error2" });
        }

        [Test]
        public void Setting_Property_Does_Not_Validate_If_Auto_Validate_Is_False()
        {
            _myValidatableScreen.AutoValidate = false;
            _myValidatableScreen.IntProperty = 5;
            _validator.Verify(x => x.ValidatePropertyAsync("IntProperty"), Times.Never);
        }

        [Test]
        public void Setting_Property_Validates_If_AutoValidate_Is_True()
        {
            _myValidatableScreen.IntProperty = 5;
            _validator.Verify(x => x.ValidatePropertyAsync("IntProperty"));
        }

        [Test]
        public void AutoValidate_Is_True_By_Default()
        {
            _myValidatableScreen.AutoValidate.ShouldBeTrue();
        }

        [Test]
        public async Task ValidateAsync_Calls_Adapter_ValidateAllPropertiesAsync()
        {
            _validator.Setup(x => x.ValidateAllPropertiesAsync())
                .ReturnsAsync(new Dictionary<string, string[]>())
                .Verifiable();

            var unused = await _myValidatableScreen.ValidateAsync();

            _validator.Verify();
        }

        [Test]
        public void Validate_Calls_Adapter_ValidateAllPropertiesAsync()
        {
            _validator.Setup(x => x.ValidateAllPropertiesAsync())
                .ReturnsAsync(
                    new Dictionary<string, string[]>
                    {
                        { "property", new[] { "error1", "error2" } }
                    })
                .Verifiable();

            var unused = _myValidatableScreen.Validate();

            _validator.Verify();
        }

        [Test]
        public async Task ValidatePropertAsync_By_Expression_Calls_Adapter_ValidatePropertAsync()
        {
            _validator.Setup(x => x.ValidatePropertyAsync("IntProperty"))
                .ReturnsAsync(Enumerable.Empty<string>().ToArray)
                .Verifiable();

            var unused = await _myValidatableScreen.ValidatePropertyAsync(() => _myValidatableScreen.IntProperty);

            _validator.Verify();
        }

        [Test]
        public Task ValidatePropertAsync_By_Expression_Throws_When_Expression_Is_Null()
        {
            return _myValidatableScreen.ValidatePropertyAsync((Expression<Func<object>>)null).ShouldThrowAsync<ArgumentNullException>();
        }

        [Test]
        public async Task ValidatePropertyAsync_By_Name_Calls_Adapter_ValidatePropertyAsync()
        {
            _validator.Setup(x => x.ValidatePropertyAsync("test"))
                .ReturnsAsync(Enumerable.Empty<string>().ToArray())
                .Verifiable();

            var unused = await _myValidatableScreen.ValidatePropertyAsync("test");

            _validator.Verify();
        }

        [Test]
        public async Task ValidatePropertyAsync_With_Empty_String_Calls_Adapter_ValidatePropertyAsync_With_Empty_String()
        {
            _validator.Setup(x => x.ValidatePropertyAsync(string.Empty))
                .ReturnsAsync(Enumerable.Empty<string>().ToArray)
                .Verifiable();

            var unused = await _myValidatableScreen.ValidatePropertyAsync(string.Empty);

            _validator.Verify();
        }

        [Test]
        public async Task ValidatePropertyAsync_With_Null_Calls_Adapter_ValidatePropertyAsync_With_Empty_String()
        {
            _validator.Setup(x => x.ValidatePropertyAsync(string.Empty))
                .ReturnsAsync(Enumerable.Empty<string>().ToArray())
                .Verifiable();

            var unused = await _myValidatableScreen.ValidatePropertyAsync(string.Empty);

            _validator.Verify();
        }

        [Test]
        public void ValidateProperty_By_Expression_Calls_Adapter_Validate()
        {
            _validator.Setup(x => x.ValidatePropertyAsync("IntProperty"))
                .ReturnsAsync(Enumerable.Empty<string>().ToArray)
                .Verifiable();

            var unused = _myValidatableScreen.ValidateProperty(() => _myValidatableScreen.IntProperty);

            _validator.Verify();
        }

        [Test]
        public void ValidateProperty_By_Expression_Throws_When_Expression_Is_Null()
        {
            Should.Throw<ArgumentNullException>(() => _myValidatableScreen.ValidateProperty((Expression<Func<object>>)null));
        }

        [Test]
        public void ValidateProperty_By_Name_Calls_Adapter_Validate()
        {
            _validator.Setup(x => x.ValidatePropertyAsync("test"))
                .ReturnsAsync(Enumerable.Empty<string>().ToArray)
                .Verifiable();

            var unused = _myValidatableScreen.ValidateProperty("test");

            _validator.Verify();
        }

        [Test]
        public void ValidateProperty_Returns_False_If_Validation_Failed()
        {
            _validator.Setup(x => x.ValidatePropertyAsync("IntProperty"))
                .ReturnsAsync(new[] { "error" });

            var result = _myValidatableScreen.ValidateProperty("IntProperty");

            result.ShouldBeFalse();
        }

        [Test]
        public void ValidateProperty_Returns_True_If_ValidationPassed()
        {
            _validator.Setup(x => x.ValidatePropertyAsync("IntProperty"))
                .ReturnsAsync(() => null);

            var result = _myValidatableScreen.ValidateProperty("IntProperty");

            result.ShouldBeTrue();

            _validator.Setup(x => x.ValidatePropertyAsync("IntProperty"))
                .ReturnsAsync(new string[0]);

            result = _myValidatableScreen.ValidateProperty("IntProperty");

            result.ShouldBeTrue();
        }

        [Test]
        public void Validate_Returns_False_If_Validation_Failed()
        {
            _validator.Setup(x => x.ValidateAllPropertiesAsync())
                .ReturnsAsync(
                    new Dictionary<string, string[]>
                    {
                        { "IntProperty", new[] { "error" } }
                    });

            var result = _myValidatableScreen.Validate();

            result.ShouldBeFalse();
        }

        [Test]
        public void Validate_Returns_True_If_Validation_Passed()
        {
            _validator.Setup(x => x.ValidateAllPropertiesAsync())
                .ReturnsAsync(
                    new Dictionary<string, string[]>
                    {
                        { "IntProperty", null }
                    });

            var result = _myValidatableScreen.Validate();

            result.ShouldBeTrue();

            _validator.Setup(x => x.ValidateAllPropertiesAsync())
                .ReturnsAsync(
                    new Dictionary<string, string[]>
                    {
                        { "IntProperty", new string[0] }
                    });

            result = _myValidatableScreen.Validate();

            result.ShouldBeTrue();
        }

        private class MyValidatableScreen : ValidatableScreen
        {
            private int _intProperty;

            public MyValidatableScreen(IValidationAdapter validationAdapter)
                : base(validationAdapter)
            {
            }

            public new bool AutoValidate
            {
                get => base.AutoValidate;
                set => base.AutoValidate = value;
            }

            public int IntProperty
            {
                get => _intProperty;
                set => Set(ref _intProperty, value);
            }

            public new bool Validate()
            {
                return base.Validate();
            }

            public new Task<bool> ValidateAsync()
            {
                return base.ValidateAsync();
            }

            public new bool ValidateProperty(string propertyName)
            {
                return base.ValidateProperty(propertyName);
            }

            public new bool ValidateProperty<TProperty>(Expression<Func<TProperty>> property)
            {
                return base.ValidateProperty(property);
            }

            public new Task<bool> ValidatePropertyAsync(string propertyName)
            {
                return base.ValidatePropertyAsync(propertyName);
            }

            public new Task<bool> ValidatePropertyAsync<TProperty>(Expression<Func<TProperty>> property)
            {
                return base.ValidatePropertyAsync(property);
            }
        }
    }
}
