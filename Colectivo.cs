using System;

public class Colectivo
{
    public decimal Tarifa { get; private set; }
    public Colectivo(decimal tarifa)
    {
        Tarifa = tarifa;
    }
    public Boleto PagarCon(Tarjeta tarjeta)
    {
        if (tarjeta.PuedePagar(Tarifa))
        {
            tarjeta.DescontarSaldo(Tarifa);
            return new Boleto(this, tarjeta, Tarifa);
        }
        else
        {
            throw new InvalidOperationException("Saldo insuficiente en la tarjeta.");
        }
    }
}

