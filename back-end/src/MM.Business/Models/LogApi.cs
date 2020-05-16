using System;

namespace MM.Business.Models
{
    public class LogApi
    {
        public LogApi(Guid? usuario_id, DateTime request_time, long response_millis, int status_code, string method, string path, string query_string, string request_body, 
            string response_body)
        {
            this.id = Guid.NewGuid();
            this.usuario_id = usuario_id;
            this.request_time = request_time;
            this.response_millis = response_millis;
            this.status_code = status_code;
            this.method = method;
            this.path = path;
            this.query_string = query_string;
            this.request_body = request_body;
            this.response_body = response_body;
        }

        public Guid id                  { get; private set; }
        public Guid? usuario_id         { get; private set; }
        public DateTime request_time    { get; private set; }
        public long response_millis     { get; private set; }
        public int status_code          { get; private set; }
        public string method            { get; private set; }
        public string path              { get; private set; }
        public string query_string      { get; private set; }
        public string request_body      { get; private set; }
        public string response_body     { get; private set; }
    }
}
