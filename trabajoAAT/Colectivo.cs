using System;
using ManejoDeTiempos;

public class Colectivo{
    public decimal Tarifa { get; private set; }
    public bool EsInterurbano { get; private set; }
    private int CantidadViajesMedioBoleto = 4;
    protected DateTime UltimoViaje { get; set; } = DateTime.MinValue;
  
    public Colectivo(bool esInterurbano = false){
        EsInterurbano = esInterurbano;
        Tarifa = esInterurbano ? 2500 : 1200;
    }

    public Boleto PagarCon(Tarjeta tarjeta,  Tiempo tiempo){
        decimal montoFinal = CalcularMontoFinal(tarjeta, tiempo);
        if (tarjeta.Saldo - montoFinal >= -480){
            tarjeta.Saldo -= montoFinal;
            tarjeta.ViajesMensuales++;
            RegistrarViaje(tiempo);
            tarjeta.CargarSaldoExcedente(tarjeta.excedente);  
          //tarjeta.Saldo = tarjeta.Saldo + tarjeta.excedente;
            return new Boleto(this, tarjeta, montoFinal, tiempo);
          
        }
        else{
            throw new InvalidOperationException("Saldo insuficiente en la tarjeta.");
        }
    }

    private decimal CalcularMontoFinal(Tarjeta tarjeta, Tiempo tiempo){
        decimal monto = Tarifa;

        if (tarjeta is TarjetaNormal normalTarjeta){
            if (normalTarjeta.ViajesMensuales >= 30 && normalTarjeta.ViajesMensuales < 79)
                monto *= 0.80m;
            else if (normalTarjeta.ViajesMensuales == 79)
                monto *= 0.75m;
        }
        else if (tarjeta is MedioBoleto medioBoleto){
          if(PuedeViajar(tiempo)){
            if(CantidadViajesMedioBoleto >0 && PuedeViajar(tiempo)){
              ValidarHorarioFranquicia(tiempo);
              monto /= 2;
              CantidadViajesMedioBoleto--;
              }
            else{
              throw new InvalidOperationException("Ya ha realizado los 4 viajes diarios permitidos");
          }
            }
          else{
            throw new InvalidOperationException("Debe esperar 5 minutos respecto al viaje anterior para volver a usar el medio boleto");
          }
          
        }
        else if (tarjeta is FranquiciaCompleta franquicia){
            ValidarHorarioFranquicia(tiempo);
            if (franquicia.ViajesMensuales < 2)
                monto = 0;
        } else {
          ValidarHorarioFranquicia(tiempo);
          monto = 0;
      }

        return monto;
    }

  private void ValidarHorarioFranquicia(Tiempo tiempo)
  {
      DateTime fechaActual = tiempo.Now(); 
      TimeSpan horaActual = fechaActual.TimeOfDay;
      DayOfWeek diaActual = fechaActual.DayOfWeek;

      // Definir el horario permitido
      TimeSpan horaInicioPermitida = new TimeSpan(6, 0, 0);
      TimeSpan horaFinPermitida = new TimeSpan(22, 0, 0);

      bool esDiaPermitido = diaActual >= DayOfWeek.Monday && diaActual <= DayOfWeek.Friday;
      bool esHoraPermitida = horaActual >= horaInicioPermitida && horaActual <= horaFinPermitida;

      if (!esDiaPermitido || !esHoraPermitida)
      {
          throw new InvalidOperationException("Fuera del horario permitido para la franquicia.");
      }
  }
  public virtual bool PuedeViajar(Tiempo tiempo){
      return (tiempo.Now() - UltimoViaje).TotalMinutes >= 5; 
  }

  public void RegistrarViaje(Tiempo tiempo){
      UltimoViaje = tiempo.Now();
     // ViajesHoy++;
  }

}