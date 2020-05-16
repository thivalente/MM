using System;

namespace MM.Business.Models
{
    public class LogErro
    {
        public LogErro(string endpoint, string mensagem_erro, string inner_exception, string stack_trace)
        {
            this.id = Guid.NewGuid();
            this.data_criacao = DateTime.Now;
            this.endpoint = endpoint;
            this.mensagem_erro = mensagem_erro;
            this.inner_exception = inner_exception;
            this.stack_trace = stack_trace;
        }

        public Guid id                  { get; private set; }
        public DateTime data_criacao    { get; private set; }
        public string endpoint          { get; private set; }
        public string mensagem_erro     { get; private set; }
        public string inner_exception   { get; private set; }
        public string stack_trace       { get; private set; }
    }
}
