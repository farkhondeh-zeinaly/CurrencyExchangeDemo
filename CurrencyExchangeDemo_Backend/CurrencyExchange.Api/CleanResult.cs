using FluentResults;

namespace CurrencyExchange.Api
{
    /// <summary>
    /// Clean result to respond to clients
    /// </summary>
    public class CleanResult
    {
        [System.Text.Json.Serialization.JsonIgnore]
        private readonly HashSet<string> _errors;

        [System.Text.Json.Serialization.JsonIgnore]
        private readonly HashSet<string> _successes;

        public bool IsSuccess { get; set; }
        public IReadOnlyList<string> Successes => _successes.ToList();

        public bool IsFailed { get; set; }
        public IReadOnlyList<string> Errors => _errors.ToList();


        public CleanResult()
        {
            _errors = new HashSet<string>();
            _successes = new HashSet<string>();
        }


        public void AddErrorMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            _errors.Add(message);
        }

        public void AddSuccessMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            _successes.Add(message);
        }
    }

    /// <summary>
    /// Clean result with data of type T
    /// </summary>
    /// <typeparam name="T">Data to transfer</typeparam>
    public class CleanResult<T> : CleanResult
    {
        public CleanResult() : base()
        {
        }
        public T? Value { get; set; }
    }

    public static class ResultExtensions
    {
        /// <summary>
        /// Converts FluentResult to CleanResult
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static CleanResult ToCleanResult(this Result result)
        {
            return result.SetMessages<Result, CleanResult>();
        }

        /// <summary>
        /// Converts FluentResult with data of type T to CleanResult with data of type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        public static CleanResult<T> ToCleanResult<T>(this Result<T> result)
        {
            var npResult = result.SetMessages<Result<T>, CleanResult<T>>();

            if (result.IsSuccess)
            {
                npResult.Value = result.Value;
            }

            return npResult;
        }

        /// <summary>
        /// Copies messages from a source of type FluentResult.IResultBase to a destination of type CleanResult
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        private static TDestination SetMessages<TSource, TDestination>(this TSource result)
            where TSource : IResultBase
            where TDestination : CleanResult, new()
        {
            var cleanResult = new TDestination
            {
                IsFailed = result.IsFailed,
                IsSuccess = result.IsSuccess
            };

            if (result.Errors is not null)
            {
                Parallel.ForEach(result.Errors, result => { cleanResult.AddErrorMessage(result.Message); });

                //foreach (var item in result.Errors)
                //{
                //    npResult.AddErrorMessage(item.Message);
                //}
            }

            if (result.Successes is not null)
            {
                Parallel.ForEach(result.Successes, result => { cleanResult.AddSuccessMessage(result.Message); });

                //foreach (var item in result.Successes)
                //{
                //    npResult.AddSuccessMessage(item.Message);
                //}
            }

            return cleanResult;
        }
    }
}
