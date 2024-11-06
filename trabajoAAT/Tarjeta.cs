using System;

public class Tarjeta{
    public decimal Saldo { get; set; }
    public int ViajesMensuales { get; set; } = 0;
    protected DateTime UltimoViaje { get; set; } = DateTime.MinValue;
    protected decimal SaldoMaximo = 36000;
    public decimal excedente;
    protected decimal SaldoNegativoPermitido = -480;
    protected decimal[] CargasAceptadas = { 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000 };

    public Tarjeta(decimal saldoInicial){
        if (saldoInicial <= SaldoMaximo){
            Saldo = saldoInicial;
        }
        else{
            throw new ArgumentOutOfRangeException("Saldo inicial excede el saldo máximo permitido.");
        }
    }
    public void CargarSaldoExcedente(decimal excedente1){
      if(Saldo + excedente1 > SaldoMaximo){
        excedente = (Saldo + excedente1) - SaldoMaximo;
        Saldo = SaldoMaximo;
      } 
      else{
        Saldo += excedente1;
      }
    }
    public void CargarSaldo(decimal monto){
        if (Array.Exists(CargasAceptadas, carga => carga == monto)){
            if (Saldo + monto > SaldoMaximo){
                excedente = (Saldo + monto) - SaldoMaximo;
                Saldo = SaldoMaximo;
                Console.WriteLine($"Se cargó hasta el máximo permitido. Excedente pendiente de acreditación: {excedente}");
            }
            else{
                Saldo += monto;
            }
        }
        else{
            throw new ArgumentException("Monto de carga no aceptado.");
        }
    }
}

public class TarjetaNormal : Tarjeta{
    public TarjetaNormal(decimal saldoInicial) : base(saldoInicial) { }
}

public class MedioBoleto : Tarjeta{
    public MedioBoleto(decimal saldoInicial) : base(saldoInicial) { }
}

public class FranquiciaCompleta : Tarjeta{
    public FranquiciaCompleta(decimal saldoInicial) : base(saldoInicial) { }
}
public class Jubilados : Tarjeta{
    public Jubilados(decimal saldoInicial) : base(saldoInicial) { }
}