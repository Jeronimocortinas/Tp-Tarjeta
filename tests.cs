using NUnit.Framework;
using ManejoDeTiempos;
using System;

namespace Tests_Tarjetas1
{
    [TestFixture] // Clase de prueba de NUnit
    public class TarjetaTests
    {
        private TiempoFalso tiempo;

        [SetUp] // Inicialización en NUnit
        public void Setup()
        {
            tiempo = new TiempoFalso();
        }

        [Test]
        public void DescontarSaldoCorrectamenteCuandoAdeuda()
        {
            tiempo.AgregarMinutos(12 * 60);
            var tarjeta = new TarjetaNormal(1000);
            tarjeta.Saldo = -400;
            var colectivo = new Colectivo();


            var exception = Assert.Throws<InvalidOperationException>(() => colectivo.PagarCon(tarjeta, tiempo));
            Assert.Multiple(() =>
            {
                Assert.That(exception.Message, Is.EqualTo("Saldo insuficiente en la tarjeta.")); // Verificar el mensaje de la excepción


                Assert.That(tarjeta.Saldo, Is.EqualTo(-400)); // Saldo no debería cambiar
            });
        }

        [Test]
        public void DescontarSaldoConCarga()
        {
            var tarjeta = new TarjetaNormal(-300);
            tarjeta.CargarSaldo(2000);

            Assert.That(tarjeta.Saldo, Is.EqualTo(2000 - 300));


        }

        [Test]
        public void PagarConSaldoSuficiente()
        {
            tiempo.AgregarMinutos(12 * 60);
            var tarjeta = new TarjetaNormal(2000);
            var colectivo = new Colectivo();

            var boleto = colectivo.PagarCon(tarjeta, tiempo);

            Assert.That(boleto, Is.Not.Null);

            Assert.That(tarjeta.Saldo, Is.EqualTo(2000 - colectivo.Tarifa));
        }



        [Test]
        public void FranquiciaCompletaSiemprePuedePagar()
        {
            tiempo.AgregarMinutos(12 * 60);
            var tarjeta = new Jubilados(2000);
            var colectivo = new Colectivo();


            var boleto = colectivo.PagarCon(tarjeta, tiempo);


            Assert.That(boleto, Is.Not.Null);
            Assert.That(tarjeta.Saldo, Is.EqualTo(2000));

            var boleto1 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.That(boleto1, Is.Not.Null);
            Assert.That(tarjeta.Saldo, Is.EqualTo(2000));

            var boleto2 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.That(boleto2, Is.Not.Null);
            Assert.That(tarjeta.Saldo, Is.EqualTo(2000));




        }


        [Test]
        public void MontoMedioBoletoEsLaMitadDeLaTarifaNormal()
        {
            tiempo.AgregarMinutos(12 * 60);
            var tarjeta = new MedioBoleto(2000);
            var colectivo = new Colectivo();

            var boleto = colectivo.PagarCon(tarjeta, tiempo);

            Assert.That(boleto.MontoPagado, Is.EqualTo(colectivo.Tarifa / 2));
        }

        [Test]
        public void TipoDeBoletoCorrectoSegunTipoDeTarjetaNormal()
        {
            var colectivo = new Colectivo();

            var tarjeta = new TarjetaNormal(2000);
            var boleto = colectivo.PagarCon(tarjeta, tiempo);

            Assert.That(boleto.TipoTarjeta, Is.EqualTo("TarjetaNormal"));
            tiempo.AgregarMinutos(12 * 60);

            
        }
        public void TipoDeBoletoCorrectoSegunTipoDeTarjetaMedio()
        {
            var colectivo = new Colectivo();

            var tarjeta = new TarjetaNormal(2000);
            var boleto = colectivo.PagarCon(tarjeta, tiempo);

            Assert.That(boleto.TipoTarjeta, Is.EqualTo("MedioBoleto"));
            
        }
        public void TipoDeBoletoCorrectoSegunTipoDeTarjetaFranquicia()
        {
            var colectivo = new Colectivo();

            var tarjeta = new TarjetaNormal(2000);
            var boleto = colectivo.PagarCon(tarjeta, tiempo);

            Assert.That(boleto.TipoTarjeta, Is.EqualTo("FranquiciaCompleta"));
            
        }
        public void TipoDeBoletoCorrectoSegunTipoDeTarjetaJubilados()
        {
            var colectivo = new Colectivo();

            var tarjeta = new TarjetaNormal(2000);
            var boleto = colectivo.PagarCon(tarjeta, tiempo);

            Assert.That(boleto.TipoTarjeta, Is.EqualTo("Jubilado"));
            
        }

        [Test]
        public void LimiteCuatroViajesPorDiaConMedioBoleto()
        {
            tiempo.AgregarMinutos(12 * 60);
            var tarjeta = new MedioBoleto(5000);
            var colectivo = new Colectivo();

            for (int i = 0; i < 4; i++)
            {
                colectivo.PagarCon(tarjeta, tiempo);
            }
            Assert.That(tarjeta.Saldo, Is.EqualTo(5000 - (colectivo.Tarifa * 2)));

        }

        [Test]
        public void FranquiciaCompletaPermiteSoloDosViajesGratuitos()
        {
            tiempo.AgregarMinutos(12 * 60);
            var tarjeta = new FranquiciaCompleta(2000);
            var colectivo = new Colectivo();

            colectivo.PagarCon(tarjeta, tiempo);
            colectivo.PagarCon(tarjeta, tiempo);

            var boleto = colectivo.PagarCon(tarjeta, tiempo);

            Assert.That(tarjeta.Saldo, Is.EqualTo(2000 - colectivo.Tarifa));
            Assert.That(boleto, Is.Not.Null);

        }

        [Test]
        public void VerificaRecargaMaxima()
        {
            var tarjeta = new TarjetaNormal(34000);
            tarjeta.CargarSaldo(2000);
            var colectivo = new Colectivo();
            Assert.That(tarjeta.Saldo, Is.EqualTo(36000));
            tarjeta.CargarSaldo(2000);
            var boleto = colectivo.PagarCon(tarjeta, tiempo);
            var boleto1 = colectivo.PagarCon(tarjeta, tiempo);
            Assert.That(boleto, Is.Not.Null);
            Assert.That(boleto1, Is.Not.Null);
            Assert.That(tarjeta.Saldo, Is.EqualTo(36000 - 400));

        }

        [Test]
        public void RestriccionesDeHorarioParaMedioBoletoYFranquiciaCompleta()
        {
            tiempo.AgregarDias(5); //sabado
                                   //tiempo.AgregarMinutos(12 * 60);

            var tarjeta = new MedioBoleto(2000);
            var colectivo = new Colectivo();

            var exception = Assert.Throws<InvalidOperationException>(() => colectivo.PagarCon(tarjeta, tiempo));
            Assert.Multiple(() =>
            {
                Assert.That(exception.Message, Is.EqualTo("Fuera del horario permitido para la franquicia."));

            });
            var tarjeta2 = new FranquiciaCompleta(2000);


            var exception2 = Assert.Throws<InvalidOperationException>(() => colectivo.PagarCon(tarjeta, tiempo));
            Assert.Multiple(() =>
            {
                Assert.That(exception.Message, Is.EqualTo("Fuera del horario permitido para la franquicia."));


            });



        }
    }
}
