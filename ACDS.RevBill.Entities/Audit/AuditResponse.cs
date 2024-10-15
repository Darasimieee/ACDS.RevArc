using System;
namespace ACDS.RevBill.Entities.Audit
{
    public class AuditResponse
    {
        public class Action
        {
            public string TraceId { get; set; }
            public string HttpMethod { get; set; }
            public string ControllerName { get; set; }
            public string ActionName { get; set; }
            public ActionParameters ActionParameters { get; set; }
            public string RequestUrl { get; set; }
            public string IpAddress { get; set; }
            public string ResponseStatus { get; set; }
            public int ResponseStatusCode { get; set; }
            public string Exception { get; set; }
        }

        public class ActionParameters
        {
            public Model model { get; set; }
        }

        public class Environment
        {
            public string UserName { get; set; }
            public string MachineName { get; set; }
            public string DomainName { get; set; }
            public string CallingMethodName { get; set; }
            public string AssemblyName { get; set; }
            public string Culture { get; set; }
        }

        public class Model
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class Root
        {
            public Action Action { get; set; }
            public string EventType { get; set; }
            public Environment Environment { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public int Duration { get; set; }
        }

    }
}