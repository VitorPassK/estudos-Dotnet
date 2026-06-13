using OlaMundo.Resources;
using OlaMundo.Services;

MensageriaSMS sms = new MensageriaSMS();
Mensageria msg = new Mensageria();

sms.EnviarMensagem("Mensagem SMS");
msg.EnviarMensagem("Mensagem Comum");