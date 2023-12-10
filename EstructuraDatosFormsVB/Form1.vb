Imports System.Collections.Generic
Imports System.Collections
Imports System.Windows.Forms
Imports System.ComponentModel

Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    End Sub

    Private tareasPendientes As New Queue(Of String)()
    Private tareasCompletadas As New List(Of String)()
    Private arbolPrioridad As SortedSet(Of String)
    Private bindingList As New BindingList(Of String)()
    Private prioridadCounter As Integer = 0

    Public Sub New()
        InitializeComponent()
        Text = "Gestión de Tareas"
        Size = New Size(800, 600)

        tareasPendientes = New Queue(Of String)()
        tareasCompletadas = New List(Of String)()
        arbolPrioridad = New SortedSet(Of String)()
        bindingList = New BindingList(Of String)()

        Dim dataList As New ListBox()
        dataList.DataSource = bindingList
        dataList.Size = New Size(600, 200) ' Ajusta el tamaño del ListBox
        dataList.DisplayMember = "NombrePropiedadMostrada" ' Ajusta la propiedad mostrada en el ListBox
        dataList.Font = New Font("Arial", 12) ' Ajusta la fuente del texto en el ListBox
        dataList.Location = New Point(400, 12) ' Cambia 400 según sea necesario

        Dim spacing As Integer = 10 ' Espacio vertical entre los botones

        Dim agregarTareaBtn As New Button()
        agregarTareaBtn.Text = "Agregar tarea pendiente (Cola)"
        agregarTareaBtn.Size = New Size(200, 40) ' Ajusta el tamaño según sea necesario
        AddHandler agregarTareaBtn.Click, AddressOf AgregarTareaBtn_Click

        Dim completarTareaBtn As New Button()
        completarTareaBtn.Text = "Completar tarea (Cola y Lista)"
        completarTareaBtn.Size = New Size(200, 100) ' Ajusta el tamaño según sea necesario
        AddHandler completarTareaBtn.Click, AddressOf CompletarTareaBtn_Click

        Dim verTareasPendientesBtn As New Button()
        verTareasPendientesBtn.Text = "Ver tareas pendientes (Cola)"
        verTareasPendientesBtn.Size = New Size(200, 40) ' Ajusta el tamaño según sea necesario
        AddHandler verTareasPendientesBtn.Click, AddressOf VerTareasPendientesBtn_Click

        Dim verTareasCompletadasBtn As New Button()
        verTareasCompletadasBtn.Text = "Ver tareas completadas (Lista)"
        verTareasCompletadasBtn.Size = New Size(200, 40) ' Ajusta el tamaño según sea necesario
        AddHandler verTareasCompletadasBtn.Click, AddressOf VerTareasCompletadasBtn_Click


        ' Ajusta las ubicaciones de los botones
        agregarTareaBtn.Location = New Point(10, 10)
        completarTareaBtn.Location = New Point(10, agregarTareaBtn.Bottom + spacing)
        verTareasPendientesBtn.Location = New Point(10, completarTareaBtn.Bottom + spacing)
        verTareasCompletadasBtn.Location = New Point(10, verTareasPendientesBtn.Bottom + spacing)


        Dim agregarTareaPrioridadBtn As New Button()
        agregarTareaPrioridadBtn.Text = "Agregar tarea al árbol de prioridad"
        agregarTareaPrioridadBtn.Size = New Size(200, 40)
        AddHandler agregarTareaPrioridadBtn.Click, AddressOf AgregarTareaPrioridadBtn_Click
        agregarTareaPrioridadBtn.Location = New Point(10, 10) ' Ajusta la ubicación según sea necesario

        Dim verArbolPrioridadBtn As New Button()
        verArbolPrioridadBtn.Text = "Ver árbol de prioridad"
        AddHandler verArbolPrioridadBtn.Click, AddressOf VerArbolPrioridadBtn_Click
        verArbolPrioridadBtn.Size = New Size(200, 40)
        verArbolPrioridadBtn.Location = New Point(agregarTareaPrioridadBtn.Right + 10, 10) ' Ajusta la ubicación según sea necesario

        Dim agregarTareaGrafoBtn As New Button()
        agregarTareaGrafoBtn.Text = "Agregar tarea al grafo de tareas"
        AddHandler agregarTareaGrafoBtn.Click, AddressOf AgregarTareaGrafoBtn_Click
        agregarTareaGrafoBtn.Size = New Size(200, 150)
        agregarTareaGrafoBtn.Location = New Point(verArbolPrioridadBtn.Right + 10, 10) ' Ajusta la ubicación según sea necesario

        Dim verGrafoBtn As New Button()
        verGrafoBtn.Text = "Ver grafo de tareas (BFS)"
        AddHandler verGrafoBtn.Click, AddressOf VerGrafoBtn_Click
        verGrafoBtn.Size = New Size(200, 40)
        verGrafoBtn.Location = New Point(agregarTareaGrafoBtn.Right + 10, 10) ' Ajusta la ubicación según sea necesario

        ' ... (Código posterior) ...
        agregarTareaPrioridadBtn.Size = New Size(200, 60)
        agregarTareaPrioridadBtn.Location = New Point(10, verTareasCompletadasBtn.Bottom + spacing)

        ' Ver árbol de prioridad
        verArbolPrioridadBtn.Size = New Size(200, 60)
        verArbolPrioridadBtn.Location = New Point(agregarTareaPrioridadBtn.Right + 10, verTareasCompletadasBtn.Bottom + spacing)

        ' Agregar tarea al grafo de tareas
        agregarTareaGrafoBtn.Size = New Size(200, 50)
        agregarTareaGrafoBtn.Location = New Point(verArbolPrioridadBtn.Right + 10, verTareasCompletadasBtn.Bottom + spacing)

        ' Ver grafo de tareas (BFS)
        verGrafoBtn.Size = New Size(200, 40)
        verGrafoBtn.Location = New Point(agregarTareaGrafoBtn.Right + 10, verTareasCompletadasBtn.Bottom + spacing)

        Me.Size = New Size(1100, 600) ' Ajusta el tamaño del formulario según sea necesario
        UpdateList()

        Dim salirBtn As New Button()
        salirBtn.Text = "Salir"
        AddHandler salirBtn.Click, AddressOf SalirBtn_Click
        salirBtn.Size = New Size(200, 40)
        salirBtn.Location = New Point(verGrafoBtn.Right + 10, verTareasCompletadasBtn.Bottom + spacing)

        Controls.AddRange({agregarTareaBtn, completarTareaBtn, verTareasPendientesBtn, verTareasCompletadasBtn,
                   agregarTareaPrioridadBtn, verArbolPrioridadBtn, agregarTareaGrafoBtn, verGrafoBtn,
                   salirBtn, dataList})
    End Sub

    Private Sub AgregarTareaBtn_Click(sender As Object, e As EventArgs)
        Dim nuevaTarea As String = InputBox("Ingrese una nueva tarea:")
        If Not String.IsNullOrEmpty(nuevaTarea) Then
            tareasPendientes.Enqueue(nuevaTarea)
            UpdateList()
        End If
    End Sub

    Private Sub CompletarTareaBtn_Click(sender As Object, e As EventArgs)
        If tareasPendientes.Count > 0 Then
            Dim tareaCompletada As String = tareasPendientes.Dequeue()
            tareasCompletadas.Add(tareaCompletada)
            UpdateList()
        Else
            MessageBox.Show("No hay tareas pendientes para completar.")
        End If
    End Sub

    Private Sub VerTareasPendientesBtn_Click(sender As Object, e As EventArgs)
        MessageBox.Show("Tareas pendientes: " & String.Join(", ", tareasPendientes))
    End Sub

    Private Sub VerTareasCompletadasBtn_Click(sender As Object, e As EventArgs)
        MessageBox.Show("Tareas completadas: " & String.Join(", ", tareasCompletadas))
    End Sub

    Private Sub AgregarTareaPrioridadBtn_Click(sender As Object, e As EventArgs)
        Dim nuevaTarea As String = InputBox("Ingrese una nueva tarea:")
        If Not String.IsNullOrEmpty(nuevaTarea) Then
            arbolPrioridad.Add(nuevaTarea)
            UpdateList()
        End If
    End Sub

    Private Sub VerArbolPrioridadBtn_Click(sender As Object, e As EventArgs)
        MessageBox.Show("Árbol de prioridad: " & String.Join(", ", arbolPrioridad))
    End Sub

    Private Sub AgregarTareaGrafoBtn_Click(sender As Object, e As EventArgs)
        AgregarTareaAlGrafo()
    End Sub

    Private Sub VerGrafoBtn_Click(sender As Object, e As EventArgs)
        Dim tareaInicial As String = InputBox("Ingrese la tarea inicial:")
        If Not String.IsNullOrEmpty(tareaInicial) Then
            Dim grafoManager As New GrafoTareasManager()
            grafoManager.AgregarTarea("Tarea A", New List(Of String) From {"Tarea B", "Tarea C"})
            grafoManager.AgregarTarea("Tarea B", New List(Of String) From {"Tarea D"})
            grafoManager.AgregarTarea("Tarea C", New List(Of String) From {"Tarea E"})
            grafoManager.AgregarTarea("Tarea D", New List(Of String)())
            grafoManager.AgregarTarea("Tarea E", New List(Of String)())

            MessageBox.Show("Recorrido BFS del grafo desde la Tarea " & tareaInicial & ":" & Environment.NewLine &
                            grafoManager.VerGrafoBFS(tareaInicial))
        End If
    End Sub

    Private Sub SalirBtn_Click(sender As Object, e As EventArgs)
        Close()
    End Sub

    Private Sub UpdateList()
        bindingList.Clear()
        bindingList.Add("Tareas Pendientes (Cola): " & String.Join(", ", tareasPendientes))
        bindingList.Add("Tareas Completadas (Lista): " & String.Join(", ", tareasCompletadas))
        bindingList.Add("Árbol de Prioridad: " & String.Join(", ", arbolPrioridad))
    End Sub

    Private Sub AgregarTareaAlGrafo()
        Dim nuevaTarea As String = InputBox("Ingrese el nombre de la nueva tarea:")
        If Not String.IsNullOrEmpty(nuevaTarea) Then
            Dim tareasRelacionadasStr As String = InputBox("Ingrese las tareas relacionadas (separadas por comas):")
            Dim tareasRelacionadas As New List(Of String)()

            If Not String.IsNullOrEmpty(tareasRelacionadasStr) Then
                tareasRelacionadas.AddRange(tareasRelacionadasStr.Split(","c))
            End If

            Dim grafoManager As New GrafoTareasManager()
            grafoManager.AgregarTarea(nuevaTarea, tareasRelacionadas)
            MessageBox.Show("Tarea agregada al grafo.")
        End If
    End Sub

    Private Class GrafoTareasManager
        Private grafo As Dictionary(Of String, List(Of String))

        Public Sub New()
            grafo = New Dictionary(Of String, List(Of String))()
        End Sub

        Public Sub AgregarTarea(tarea As String, tareasRelacionadas As List(Of String))
            grafo(tarea) = New List(Of String)(tareasRelacionadas)
        End Sub

        Public Function VerGrafoBFS(tareaInicial As String) As String
            Dim visitados As New HashSet(Of String)()
            Dim cola As New Queue(Of String)()

            cola.Enqueue(tareaInicial)
            visitados.Add(tareaInicial)

            Dim result As New List(Of String)()

            While cola.Count > 0
                Dim tareaActual As String = cola.Dequeue()
                result.Add(tareaActual)

                For Each tareaRelacionada As String In grafo.GetValueOrDefault(tareaActual, New List(Of String)())
                    If Not visitados.Contains(tareaRelacionada) Then
                        cola.Enqueue(tareaRelacionada)
                        visitados.Add(tareaRelacionada)
                    End If
                Next
            End While

            Return String.Join(", ", result)
        End Function
    End Class
End Class