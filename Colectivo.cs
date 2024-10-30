using System;

public class Colectivo{
    public decimal Tarifa { get; private set; }
    public bool EsInterurbano { get; private set; }

    public Colectivo(bool esInterurbano = false){
        EsInterurbano = esInterurbano;
        Tarifa = esInterurbano ? 2500m : 1200m;
    }

    public Boleto PagarCon(Tarjeta tarjeta){
        decimal montoFinal = CalcularMontoFinal(tarjeta);

        if (tarjeta.Saldo - montoFinal >= -480){
            tarjeta.Saldo -= montoFinal;
            tarjeta.ViajesMensuales++;
            return new Boleto(this, tarjeta, montoFinal);
        }
        else{
            throw new InvalidOperationException("Saldo insuficiente en la tarjeta.");
        }
    }

    private decimal CalcularMontoFinal(Tarjeta tarjeta){
        decimal monto = Tarifa;

        if (tarjeta is TarjetaNormal normalTarjeta){
            if (normalTarjeta.ViajesMensuales >= 30 && normalTarjeta.ViajesMensuales < 79)
                monto *= 0.80m;
            else if (normalTarjeta.ViajesMensuales == 79)
                monto *= 0.75m;
        }
        else if (tarjeta is MedioBoleto medioBoleto){
            ValidarHorarioFranquicia();
            monto /= 2;
        }
        else if (tarjeta is FranquiciaCompleta franquicia){
            ValidarHorarioFranquicia();
            if (franquicia.ViajesMensuales < 2)
                monto = 0;
        }

        return monto;
    }

    private void ValidarHorarioFranquicia(){
        var hora = DateTime.Now.TimeOfDay;
        var dia = DateTime.Now.DayOfWeek;
        bool esHorarioPermitido = dia >= DayOfWeek.Monday && dia <= DayOfWeek.Friday && hora >= new TimeSpan(6, 0, 0) && hora <= new TimeSpan(22, 0, 0);

        if (!esHorarioPermitido)
            throw new InvalidOperationException("Fuera del horario permitido para la franquicia.");
    }
}

