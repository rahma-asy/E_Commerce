using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace E_Commerce.Application.Comman
{
    public class Result
    {
        public bool IsSuccess { get; }//can't change it unless using helper method

        public IReadOnlyList<Error> Errors { get; }
        //available for childs
        internal Result(bool isSuccess, IReadOnlyList<Error> errors)
        {
            IsSuccess = isSuccess;
            Errors = errors;
        }

        public static Result Ok() => new(true, Array.Empty<Error>());

        public static Result Fail(Error error) => new(false, new[] { error });

        public static Result Fail(IReadOnlyList<Error> errors) => new(false, errors);
    }

    public class Result<TValue> : Result
    {
        private readonly TValue? _value;

        public TValue Value => IsSuccess ? _value! : throw new InvalidOperationException("Can Not Access The Value Of Failed Result");

        private Result(TValue value) : base(true, Array.Empty<Error>())
        {
            _value = value;
        }

        private Result(Error error) : base(false, new[] { error })
        {
            _value = default!;
        }

        private Result(IReadOnlyList<Error> errors) : base(false, errors)
        {
            _value = default!;
        }

        public static Result<TValue> Ok(TValue value) => new Result<TValue>(value);

        public static Result<TValue> Fail(Error error) => new Result<TValue>(error);

        public static Result<TValue> Fail(IReadOnlyList<Error> errors) => new Result<TValue>(errors);

        //if he send value i will know that he want to return Ok(value)
        public static implicit operator Result<TValue>(TValue value) => Ok(value);
        //if he send error i will know that he want to return Fail(error)
        public static implicit operator Result<TValue>(Error error) => Fail(error);
    }
}
    

