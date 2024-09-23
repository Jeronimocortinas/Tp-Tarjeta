using System;
public class Boleto
{
    public Colectivo Colectivo { get; private set; }
    public Tarjeta Tarjeta { get; private set; }
    public decimal MontoPagado { get; private set; }
    public DateTime FechaHora { get; private set; }

    public Boleto(Colectivo colectivo, Tarjeta tarjeta, decimal montoPagado)
    {
        Colectivo = colectivo;
        Tarjeta = tarjeta;
        MontoPagado = montoPagado;
        FechaHora = DateTime.Now;
    }
    public override string ToString()
    {
        return $"Boleto generado:\nMonto: {MontoPagado}\nSaldo Restante: {Tarjeta.Saldo}";
    }
}
