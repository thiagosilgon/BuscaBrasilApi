using BuscaBrasilApi.Interfaces;
using BuscaBrasilApi.Model;

namespace BuscaBrasilApi.Factories
{
    abstract class FConfiguracao
    {
        public abstract IConfiguracao Build(string ambiente);
    }

    class FactoryConfiguracao : FConfiguracao
    {
        public override IConfiguracao Build(string ambiente)
        {
            return new Configuracao(ambiente);
        }
    }
}
