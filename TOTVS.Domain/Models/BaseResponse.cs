using System.Collections.Generic;

namespace TOTVS.Domain.Models
{
    public class BaseResponse
    {
        private List<ErrorModel> _errors;

        public BaseResponse()
        {
            _errors = new List<ErrorModel>();
        }

        public bool IsSuccessful { get; set; }

        public List<ErrorModel> Errors
        {
            get => _errors;
            private set => _errors = value;
        }

        public void AddError(ErrorModel error)
        {
            _errors.Add(error);
        }

        public void AddError(int errorStatusCode, string errorMessage)
        {
            var error = new ErrorModel() { ErrorMessage = errorMessage, ErrorStatusCode = errorStatusCode };
            _errors.Add(error);
        }
    }
}
