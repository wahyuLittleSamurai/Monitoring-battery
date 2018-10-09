Imports MySql.Data.MySqlClient
Imports System
Imports System.Threading
Imports System.IO.Ports
Imports System.ComponentModel

Public Class Form1
    Dim mySqlConn As MySqlConnection
    Dim command As MySqlCommand
    Dim myPort As Array
    Delegate Sub SetTextCallBack(ByVal [text] As String)
    Private Sub initGraph()
        Chart2.Series.Add("ampere")
        Chart1.Series.Add("voltage")
        Chart2.Series("ampere").Color = Color.Red
        Chart1.Series("voltage").Color = Color.Blue
        Chart1.Series("voltage").ChartType = DataVisualization.Charting.SeriesChartType.Line
        Chart2.Series("ampere").ChartType = DataVisualization.Charting.SeriesChartType.Line

    End Sub
    Private Sub loadGraph()

        Dim read As MySqlDataReader
        mySqlConn = New MySqlConnection
        mySqlConn.ConnectionString = "server=localhost;userid=root;password=;database=dbpakbudi"

        Try
            mySqlConn.Open()
            Dim query As String
            query = "SELECT * FROM dbpakbudi.tblbattery"
            command = New MySqlCommand(query, mySqlConn)
            read = command.ExecuteReader
            While read.Read
                Chart2.Series("ampere").Points.AddXY(read("time").ToString, read("ampere").ToString)
                Chart1.Series("voltage").Points.AddXY(read("time").ToString, read("voltage").ToString)
            End While
            mySqlConn.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        SerialPort1.Close()
        Me.Close()

    End Sub

    Private Sub Chart1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart1.Click

    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub
    Private Sub SerialPort1_DataReceived(ByVal sender As System.Object, ByVal e As System.IO.Ports.SerialDataReceivedEventArgs) Handles SerialPort1.DataReceived
        ReceivedText(SerialPort1.ReadExisting())
    End Sub
    Private Sub ReceivedText(ByVal [text] As String)
        Dim charToArrayFunc As String
        If Me.TextBox2.InvokeRequired Then
            Dim x As New SetTextCallBack(AddressOf ReceivedText)
            Me.Invoke(x, New Object() {(text)})
        Else
            Me.TextBox2.Text = [text]


            charToArrayFunc = TextBox2.Text
           
            Dim strArr() As String
            strArr = charToArrayFunc.Split("#")

            Label2.Text = strArr(1)
            Label4.Text = strArr(0)

            mySqlConn = New MySqlConnection
            mySqlConn.ConnectionString = "server=localhost;userid=root;password=;database=dbpakbudi"
            Dim read As MySqlDataReader
            Try
                mySqlConn.Open()
                Dim query As String
                query = "INSERT INTO dbpakbudi.tblbattery (ampere,voltage) VALUES('" & strArr(0) & "','" & strArr(1) & "')"
                command = New MySqlCommand(query, mySqlConn)
                read = command.ExecuteReader

                'MessageBox.Show("Data sudah disimpan", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)

                mySqlConn.Close()
                loadGraph()
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        End If

    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        SerialPort1.PortName = ComboBox1.Text
        SerialPort1.BaudRate = ComboBox2.Text
        SerialPort1.Open()
        MessageBox.Show("open")
        'SerialPort1.Close()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged

    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged

    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        initGraph()

        myPort = IO.Ports.SerialPort.GetPortNames()
        ComboBox1.Items.AddRange(myPort)
    End Sub

    Private Sub Label3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label3.Click

    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        mySqlConn = New MySqlConnection
        mySqlConn.ConnectionString = "server=localhost;userid=root;password=;database=dbpakbudi"
        Dim read As MySqlDataReader
        Try
            mySqlConn.Open()
            Dim query As String
            query = "DELETE FROM dbpakbudi.tblbattery WHERE 1"
            command = New MySqlCommand(query, mySqlConn)
            read = command.ExecuteReader

            MessageBox.Show("Reset Data", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)

            mySqlConn.Close()
            loadGraph()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        SerialPort1.Write("S")
    End Sub

    Private Sub Label2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label2.Click

    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        SerialPort1.Write("C")
    End Sub

    Private Sub Button4_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        SerialPort1.Write("D")
    End Sub
End Class
