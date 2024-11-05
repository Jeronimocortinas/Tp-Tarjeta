using System;
using ManejoDeTiempos;

public class Boleto
{
    public Colectivo Colectivo { get; private set; }
    public Tarjeta Tarjeta { get; private set; }
    public decimal MontoPagado { get; private set; }
    public DateTime FechaHora { get; private set; }
    public string TipoTarjeta => Tarjeta.GetType().Name;
    public string LineaColectivo => Colectivo.EsInterurbano ? "Línea Interurbana" : "Línea Urbana";
    public string IdTarjeta => Tarjeta.GetHashCode().ToString();
    public decimal SaldoRestante => Tarjeta.Saldo;

    public Boleto(Colectivo colectivo, Tarjeta tarjeta, decimal montoPagado, Tiempo tiempo){
        Colectivo = colectivo;
        Tarjeta = tarjeta;
        MontoPagado = montoPagado;
        FechaHora = tiempo.Now();
    }

    public override string ToString(){
        string cancelacionSaldo = Tarjeta.Saldo < 0 ? $"Abona saldo {-Tarjeta.Saldo}" : "Sin saldo negativo";
        return $"Boleto generado:\nMonto: {MontoPagado}\nSaldo Restante: {SaldoRestante}\nTipo de tarjeta: {TipoTarjeta}\nLínea de colectivo: {LineaColectivo}\nID de tarjeta: {IdTarjeta}\nFecha: {FechaHora}\n{cancelacionSaldo}";
    }
}