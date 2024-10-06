using System;
using System.Collections.Generic;

public class Tarjeta{
    public decimal Saldo { get; protected set; }
    public int viajesMedioBoleto = 4;
    private readonly decimal SaldoMaximo = 9900;
    private readonly decimal SaldoNegativoPermitido = -480;
    private readonly decimal[] CargasAceptadas = { 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000 };

    protected int ViajesHoy { get; set; } = 0;
    protected DateTime UltimoViaje { get; set; } = DateTime.MinValue;

    public Tarjeta(decimal saldoInicial){
        if (saldoInicial <= SaldoMaximo){
            Saldo = saldoInicial;
        }
        else{
            throw new ArgumentOutOfRangeException("Saldo inicial excede el saldo máximo permitido.");
        }
    }

    public virtual bool PuedePagar(decimal monto){
        return Saldo - monto >= SaldoNegativoPermitido;
    }

    public virtual void DescontarSaldo(decimal monto){
        if (PuedePagar(monto)){
            Saldo -= monto;
        }
        else{
            throw new InvalidOperationException("La tarjeta no puede tener menos saldo que el permitido (-480).");
        }
    }

    public void CargarSaldo(decimal monto){
        if (Array.Exists(CargasAceptadas, carga => carga == monto)){
            if (Saldo + monto > SaldoMaximo){
                decimal excedente = (Saldo + monto) - SaldoMaximo;
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

    public virtual bool PuedeViajar(){
        return (DateTime.Now - UltimoViaje).TotalMinutes >= 5; 
    }

    public void RegistrarViaje(){
        UltimoViaje = DateTime.Now;
        ViajesHoy++;
    }

    public bool TieneViajesHoyExcedidos(){
        return ViajesHoy >= 4;
    }
}

public class MedioBoleto : Tarjeta{
    private int contadorViajesMedioBoleto = 0;

    public MedioBoleto(decimal saldoInicial) : base(saldoInicial){}

    public override bool PuedePagar(decimal monto){
        return base.PuedePagar(monto / (contadorViajesMedioBoleto < viajesMedioBoleto ? 2 : 1));
    }

    public override void DescontarSaldo(decimal monto){
        if (PuedeViajar()){
            if (contadorViajesMedioBoleto < viajesMedioBoleto){
                base.DescontarSaldo(monto / 2);
                contadorViajesMedioBoleto++;
            }
            else{
                base.DescontarSaldo(monto);
            }
            RegistrarViaje();
        }
        else{
            throw new InvalidOperationException("No se puede realizar un nuevo viaje antes de 5 minutos.");
        }
    }
}