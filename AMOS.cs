//Librerias utilizadas 
using System;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Drawing;
using System.Linq;

//nombre del proyecto 
namespace SerialReceive
{
    public partial class PROYECTO_SERIAL : System.Windows.Forms.Form
    {


        //Este es un list privado para que siempre tenga los valores sea cual sea la acción que se realice
        //O al menos en esta clase
        private List<Datos> datosDeTxt = new List<Datos>();
        //Variable para dar acceso al form 
        private delegate void DelegadoAcceso(string accion);
        //Variables para la lectura de datos
        private string strBufferIn;
        private string strBufferOut;

        //Esta es para tomar la fecha, hora al momento de conectarse al puerto
        private DateTime fecha = DateTime.Now;
        //Es el contador que vamos a usar por mientras
        //private int aumentoTiempo = 0;
        //Es el contador que vamos a usar por mientras
        private TimeSpan aumentoTiempo = new TimeSpan(0, 1, 0);
        //Variable para abrir ventana de busqueda de archivos 
        FolderBrowserDialog fbd = new FolderBrowserDialog();
       private string ruta; 
        //Inicio de la clase del diseño  

        public PROYECTO_SERIAL()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Esta función es para validar si entra o no en el rango de tiempo
        /// </summary>
        /// <param name="aumento">es el aumento en minutos que se tendra que sumar a la hora anterior</param>
        public bool MandarDatos(TimeSpan aumento)
        {

            //Se usa para validar el minuto actual 
            System.Diagnostics.Debug.WriteLine("Minuto anterior: " + fecha.ToString("mm") + "  Minuto actual: " + DateTime.Now.ToString("mm"));

            if ((fecha + aumentoTiempo).ToString("mm") == DateTime.Now.ToString("mm"))
            {
                fecha = fecha + aumentoTiempo;
                //MessageBox.Show("aqui voy");
                return true;
            }

            else
                return false;

        }
        //---------------------------Metodo para leer dato e insertar en un .txt-------------

        public void LeerDatos(Datos datoRecibido)
        {
            ruta = txtBrowse.Text;
            StreamWriter sw = new StreamWriter(ruta);
            //Primero se agrega el dato a la lista
            datosDeTxt.Add(datoRecibido);

            //Luego la lista se cambia a el sentido contrario, esto para que el ultimo dato ingresado salga al inicio del txt
            sw.WriteLine(datosDeTxt.ToString());

            //Iniciamos el StreamWriter que es el motor para crear el archivo, se especifica la ruta donde se almacenara
            // System.IO.StreamWriter stre = new System.IO.StreamWriter(Server.MapPath("C:/Datos/datos.txt") + "datos.txt", true); 
          

           //Creamos el encabezado
           sw.WriteLine("+-------------+-------------+-------------+-------------+-------------+-------------+-------------+-------------+-------------+------------+-------------+-------------+-------------+-------------+");
           sw.WriteLine("|Caja 1       | Caja 2      |  Caja 3     |  Caja 4     | Caja 5      | Caja 6      | Caja 7      |Caja 8       |   Caja 9    |Caja 10     |TEMPERATURA  | LUMINOSIDAD |    FECHA    |    HORA     |");
           sw.WriteLine("+-------------+-------------+-------------+-------------+-------------+-------------+-------------+-------------+-------------+------------+-------------+-------------+-------------+-------------+");

            //Creamos un foreach para recorrer la lista inversa y separar los datos del mismo
            foreach (var dato in datosDeTxt)
            {
                string espaciado = "";
                //Cada valor separado se agrega a una linea en el archivo

                for (int i = 0; i < 13 - (dato.Temp.Length + 1); i++)
                    espaciado = espaciado + " ";

                sw.WriteLine("|" + espaciado + dato.Caja1 + " " + espaciado + dato.Caja2 + " " + espaciado + dato.Caja3 + " " + espaciado + dato.Caja4 + " " +espaciado + dato.Caja5+ ""+espaciado + dato.Caja6 + "" + espaciado + dato.Caja7+ " "+ espaciado + dato.Caja8 + " "+  espaciado + dato.Caja9 + " "+ espaciado + dato.Caja10 + ""+ espaciado + dato.Temp + " " + espaciado + dato.Lum + "," + " " + dato.Fecha + "," + " " + dato.Hora + "|");
            }
            sw.WriteLine("+-------------+-------------+-------------+-------------+-------------+-------------+-------------+-------------+");

            //Cerramos el motor para que se guarden los cambios en el archivo
            

            //Volvemos a invertir la lista para que el siguiente dato sigua la linea de la lista correctamente
            sw.WriteLine(datosDeTxt.ToString());
            sw.Close();

        }//cierra metodo leer dato 

        //metodo para dar acceso al form
        private void AccesoForm(string accion)
        {
            //--------------------- tener  acceso a los datos que se ingresan desde el puerto serial

            //El numero 10 es provisional

            if (MandarDatos(new TimeSpan(0,1,0)))
            {
                try
                {
                    strBufferIn = accion;
                    

                    //Aqui es donde se hara la separación de datos

                    System.Diagnostics.Debug.WriteLine(accion);

                    string  caja1,caja2,caja3,caja4,caja5,caja6,caja7,caja8,caja9,caja10,temp, lum;
                    //Se hace un array el cual se separa por la coma para eso funciona la herramienta split 
                    //(Puede ser cualquier parametro desde una palabra hasta caracteres especiales en un orden especifico)
                    string[] subStrings = accion.Split(',');

                    caja1 = subStrings[0];
                    caja2 = subStrings[1];
                    caja3 = subStrings[2];
                    caja4 = subStrings[3];
                    caja5 = subStrings[4];
                    caja6 = subStrings[5];
                    caja7 = subStrings[6];
                    caja8 = subStrings[7];
                    caja9 = subStrings[8];
                    caja10 = subStrings[9];
                    temp = subStrings[10];
                    lum = subStrings[11];

                    txtDatoRecibido.Text = accion;
                    spPuertoSerial.DiscardInBuffer();

                    Datos datosNuevos = new Datos();
                    datosNuevos.Caja1 = caja1;
                    datosNuevos.Caja2 = caja2;
                    datosNuevos.Caja3 = caja3;
                    datosNuevos.Caja4 = caja4;
                    datosNuevos.Caja5 = caja5;
                    datosNuevos.Caja6 = caja6;
                    datosNuevos.Caja7 = caja7;
                    datosNuevos.Caja8 = caja8;
                    datosNuevos.Caja9 = caja9;
                    datosNuevos.Caja10 = caja10;
                    datosNuevos.Temp = temp;
                    datosNuevos.Lum = lum;
                    datosNuevos.Fecha = DateTime.Now.ToShortDateString();
                    datosNuevos.Hora = DateTime.Now.ToLongTimeString();

                    LeerDatos(datosNuevos);
                    //Este metodo ingresa datos al gridView y lo agrega, no se puede poner un objeto, se tiene que poner separado por comas
                    //Asi separamos el objeto actual en los elementos separados
                    dataGridView1.Rows.Add(datosNuevos.Caja1, datosNuevos.Caja2,datosNuevos.Caja3,datosNuevos.Caja4, datosNuevos.Caja5, datosNuevos.Caja6, datosNuevos.Caja7, datosNuevos.Caja8, datosNuevos.Caja9, datosNuevos.Caja10,datosNuevos.Temp, datosNuevos.Lum, datosNuevos.Fecha, datosNuevos.Hora);
                    //spPuertoSerial.DiscardOutBuffer();

                    //System.Diagnostics.Debug.WriteLine("1");

                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message.ToString());
                }
            }
        }


            

        //Metodo para estar guardando los datos 
        private void AccesoInterrupcion(string accion)
        {
            try
            {
                DelegadoAcceso var_DA;
                var_DA = new DelegadoAcceso(AccesoForm);


                object[] arg = { accion };

                base.Invoke(var_DA, arg);
            }

            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("No toma el valor o supera el buffer asignado");
            }

        }

        //Metodo para poder leer datos del serial 
        private void PROYECTO_SERIAL_Load(object sender, EventArgs e)
        {
            //strBufferIn = "";
            //strBufferOut = "";
            btnConectar.Enabled = false;
        }

        //Metodo  y acción para buscar los puertos disponibles en la pc 
        private void btnBuscar_Puertos_Click(object sender, EventArgs e)
        {
            // en un arreglo se guardarn los datos disponobles 
            string[] PuertosDisponibles = SerialPort.GetPortNames();
            cmbPuertos.Items.Clear();

            foreach (string puerto in PuertosDisponibles)
            {
                // en el combo box puertos 
                // se guardaran los puertos disponibles 
                cmbPuertos.Items.Add(puerto);

            }
            //si el cmbPuertos es mayor que 0 se mostrará  el siguiente mensaje 
            if (cmbPuertos.Items.Count > 0)
            {
                cmbPuertos.SelectedIndex = 0;
                //-------------------------------Mensaje para seleccionar el puerto 
                // Esto nos indica que el metodo para encontrar los seriales esta funcionando 
                MessageBox.Show("Seleccionar el puerto");

                btnConectar.Enabled = true;
                //------------------------------------------------------------------------------
            }
            //si no se encuentra ningun puerto pasara al siguiente else 
            else
            {
                MessageBox.Show("Ningun puerto seleccionado");
                cmbPuertos.Items.Clear();
                cmbPuertos.Text = "                       ";
               // strBufferIn = "";
               // strBufferOut = "";
                btnConectar.Enabled = false;
            }
        }

        /*
         *En el botón conectar le pasaremos los 
         * parametros que necesita para recibir los datos 
         * 
         */
        private void btnConectar_Click(object sender, EventArgs e)
        {
            
            // La función y el metodo para conectra con el serial
            try
            {
                if (btnConectar.Text == "CONECTAR")
                {
                    
                    //datos para poder conectar con el serial 
                    spPuertoSerial.BaudRate = 9600;
                    spPuertoSerial.DataBits = 8;
                    spPuertoSerial.Parity = Parity.None;
                    spPuertoSerial.StopBits = StopBits.One;
                    spPuertoSerial.Encoding = Encoding.ASCII;
                    tTime.Interval = 10000;
                    tTime.Enabled = true;
                    spPuertoSerial.PortName = cmbPuertos.Text;
                    spPuertoSerial.Open();
                    spPuertoSerial.Write("1");

                    System.Diagnostics.Debug.WriteLine(strBufferOut);

                    AccesoInterrupcion(spPuertoSerial.ReadExisting());

                    System.Diagnostics.Debug.WriteLine(strBufferIn);
                 

                    try
                    {
                        //Línea de código para abrir el puerto serial 
                        
 
                        /*
                         * Cuando se conecte al serial 
                         * este mandara un dato de 
                         * para concer que si esta concetado 
                         * 
                         */
                        btnConectar.Text = "DESCONECTAR";

                        //Lo que se hace es tomar toda la fecha, dia/mes/año hh:mm:ss por cualquier cosa que quieras hacer despues
                        //Luego lo que hago es separar esa fecha en solo los minutos para comenzar a hacer el contador 
                        fecha = DateTime.Now;
                        // aumentoTiempo = Int32.Parse(fecha.ToString("mm"));
                    }
                    catch (Exception EXC)
                    {
                        MessageBox.Show(EXC.Message.ToString());
                    }
                }
                //EL else if es para cambiar el botn de conectar por las palabras "desconectar"
                else if (btnConectar.Text == "DESCONECTAR")
                {
                    spPuertoSerial.Close();
                    btnConectar.Text = "CONECTAR";
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message.ToString());
            }
        }

        //*Metodo para recibir los datos del serial 

        private void spPuertoSerial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
                try
                {
                    Thread.Sleep(2000);
                    string dato = spPuertoSerial.ReadExisting();
                    AccesoInterrupcion(dato);
                }


                catch (Exception ec)
                {
                    MessageBox.Show(ec.Message.ToString());
                }
        }

        private void lbListaDatos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        //
        private void txtDatoRecibido_TextChanged(object sender, EventArgs e)
        {


        }

        private void cmbPuertos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        //función para agregar el timer 
        private void tTime_Tick(object sender, EventArgs e)
        {
            try
            {
                
               
                lblTiempo.Text = DateTime.Now.ToLongTimeString();

                tTime.Start();
                if ((fecha + aumentoTiempo).ToString("mm") == DateTime.Now.ToString("mm"))
                {
                    //spPuertoSerial.DiscardInBuffer();
                    //spPuertoSerial.DiscardOutBuffer();
                    spPuertoSerial.Write("1");
                    Thread.Sleep(1000);
                    System.Diagnostics.Debug.WriteLine(strBufferOut);

                    spPuertoSerial.DiscardOutBuffer();
                   
                }



            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Truena por el minuto" + ex);
            }

        }

        private void fechaPrieba_Click(object sender, EventArgs e)
        {

        }

        

        //Metodo para seleccionar donde guardar el archivo generado por el sistema 

        private void btnArchivos_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog(); 
        }

        public class Datos
        {
            public string Caja1 { get; set;}
            public string Caja2 { get; set; }
            public string Caja3 { get; set; }
            public string Caja4 { get; set; }
            public string Caja5 { get; set; }
            public string Caja6 { get; set; }
            public string Caja7 { get; set; }
            public string Caja8 { get; set; }
            public string Caja9 { get; set; }
            public string Caja10 { get; set; }
            public string Temp { get; set; }
            public string Lum { get; set; }
            public string Fecha { get; set; }
            public string Hora { get; set; }
        }

        private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            txtBrowse.Text = saveFileDialog1.FileName;
        }

        private void btnOneMinute_Click(object sender, EventArgs e)
        {

        }

        
    }
}

