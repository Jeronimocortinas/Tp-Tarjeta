using System;

public class Colectivo{
    public decimal Tarifa { get; private set; }

    public Colectivo(decimal tarifa){
        Tarifa = tarifa;
    }

    public Boleto PagarCon(Tarjeta tarjeta){
        if (tarjeta.Saldo >= Tarifa){
            tarjeta.DescontarSaldo(Tarifa);
            return new Boleto(this, tarjeta, Tarifa);
        }
        else{
            throw new InvalidOperationException("Saldo insuficiente en la tarjeta.");
        }
    }
}

public class Tarjeta{
    public decimal Saldo { get; private set; }
    private readonly decimal SaldoMaximo = 9900;
    private readonly decimal[] CargasAceptadas = { 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000 };

    public Tarjeta(decimal saldoInicial){
        if (saldoInicial <= SaldoMaximo){
            Saldo = saldoInicial;
        }
        else{
            throw new ArgumentOutOfRangeException("Saldo inicial excede el saldo máximo permitido.");
        }
    }

    public void CargarSaldo(decimal monto){
        if (Array.Exists(CargasAceptadas, carga => carga == monto)){
            if (Saldo + monto <= SaldoMaximo){
                Saldo += monto;
            }
            else{
                throw new InvalidOperationException("La carga excede el saldo máximo permitido.");
            }
        }
        else{
            throw new ArgumentException("Monto de carga no aceptado.");
        }
    }

    public void DescontarSaldo(decimal monto){
        Saldo -= monto;
    }
}

public class Boleto{
    public Colectivo Colectivo { get; private set; }
    public Tarjeta Tarjeta { get; private set; }
    public decimal MontoPagado { get; private set; }
    public DateTime FechaHora { get; private set; }

    public Boleto(Colectivo colectivo, Tarjeta tarjeta, decimal montoPagado){
        Colectivo = colectivo;
        Tarjeta = tarjeta;
        MontoPagado = montoPagado;
        FechaHora = DateTime.Now;
    }

    public override string ToString(){
        return $"Boleto generado:\nMonto: {MontoPagado}\nSaldo Restante: {Tarjeta.Saldo}";
    }
}

class Program{
    static void Main(string[] args){
        
            Tarjeta tarjeta = new Tarjeta(3000);
            Console.WriteLine($"Saldo inicial en la tarjeta: {tarjeta.Saldo}");

            Colectivo colectivo = new Colectivo(940);

            Boleto boleto = colectivo.PagarCon(tarjeta);

            Console.WriteLine(boleto);

            tarjeta.CargarSaldo(5000m);
            Console.WriteLine($"Saldo después de la carga: {tarjeta.Saldo}");

        }
}
