using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Helpers.ErrorHandling 
{
    public class ResultOrError
    {
        public bool Success { get; protected set; } = true;
        public List<string> ErrorMessages { get; private set; } = new List<string>();
        public string ErrorMessage => string.Join("\n", ErrorMessages);

        public void AddError(string message)
        {
            Success = false;
            ErrorMessages.Add(message);
        }

        public static implicit operator bool(ResultOrError result) => result.Success;

    }

    public class ResultOrError<T> : ResultOrError
    {
        public T Value { get; set; }

        public static ResultOrError<T> CreateSuccess(T value)
        {
            return new ResultOrError<T> { Value = value };
        }

        public static new ResultOrError<T> CreateFailure(string message)
        {
            var result = new ResultOrError<T>();
            result.AddError(message);
            return result;
        }

        public static implicit operator bool(ResultOrError<T> result) => result.Success;
    }
}