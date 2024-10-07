using System;
using System.Collections.Generic;

public class Tarjeta{
    public decimal Saldo { get; protected set; }
    public int viajesMedioBoleto = 4;
    private readonly decimal SaldoMaximo = 36000;
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
        return (DateTime.Now - UltimoViaje).TotalMinutes >= 5; // Puede viajar si han pasado 5 minutos
    }

    public void RegistrarViaje(){
        UltimoViaje = DateTime.Now;
        ViajesHoy++;
    }

    public bool TieneViajesHoyExcedidos(){
        return ViajesHoy >= 4; // Puede realizar hasta 4 viajes
    }
}

public class MedioBoleto : Tarjeta{
    private int contadorViajesMedioBoleto = 0;

    public MedioBoleto(decimal saldoInicial) : base(saldoInicial){
    }

    public override bool PuedePagar(decimal monto){
        // El medio boleto paga la mitad
        return base.PuedePagar(monto / (contadorViajesMedioBoleto < viajesMedioBoleto ? 2 : 1));
    }

    public override void DescontarSaldo(decimal monto){
        if (PuedeViajar()){
            if (contadorViajesMedioBoleto < viajesMedioBoleto){
                base.DescontarSaldo(monto / 2); // Descontamos la mitad
                contadorViajesMedioBoleto++;
            }
            else{
                base.DescontarSaldo(monto); // Descontamos el monto completo
            }
            RegistrarViaje();
        }
        else{
            throw new InvalidOperationException("No se puede realizar un nuevo viaje antes de 5 minutos.");
        }
    }
}

public class FranquiciaCompleta : Tarjeta{
    private int viajesGratuitosHoy;

    public FranquiciaCompleta(decimal saldoInicial) : base(saldoInicial){
        viajesGratuitosHoy = 0;
    }

    public override bool PuedePagar(decimal monto){
        if (viajesGratuitosHoy < 2){
            return true; // Puede pagar (es gratis)
        }
        else{
            return base.PuedePagar(monto); // Después de los 2 viajes, se cobra el monto completo
        }
    }

    public override void DescontarSaldo(decimal monto){
        if (viajesGratuitosHoy < 2){
            viajesGratuitosHoy++;
        }
        else{
            base.DescontarSaldo(monto);
        }
    }

    public void ReiniciarViajesHoy(){
        viajesGratuitosHoy = 0;
    }
}