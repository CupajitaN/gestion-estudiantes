import { Component, OnInit } from '@angular/core';
import { UsuarioService } from '../../../../core/services/usuario.service';
import { EstudianteService } from '../../../../core/services/estudiante.service';
import { ProfesorService } from '../../../../core/services/profesor.service';
import Swal from 'sweetalert2';

@Component({
  standalone: false,
  selector: 'app-usuarios',
  templateUrl: './usuarios.component.html',
  styleUrls: ['./usuarios.component.css'],
})
export class UsuariosComponent implements OnInit {
  usuarios: any[] = [];

  nuevoUsuario: any = {
    correo: '',
    rol: ''
  };

  formularioVisible = false;
  usuarioEditando: any = null;

  estudiantes: any[] = [];
  docentes: any[] = [];

  page = 1;
  pageSize = 10;
  totalItems = 0;
  pageSizeOptions = [5, 10, 20, 50];
  
  constructor(
    private usuarioService: UsuarioService,
    private estudianteService: EstudianteService,
    private profesorService: ProfesorService
  ) {}

  ngOnInit(): void {
    this.cargarUsuarios();
    this.estudianteService.obtenerEstudiantes().subscribe(e => this.estudiantes = e);
    this.profesorService.obtenerDocentes().subscribe(d => this.docentes = d);
  }

  cargarUsuarios() {
    this.usuarioService.obtenerTodos(this.page, this.pageSize).subscribe(res => {
      this.usuarios = res.data;
      this.totalItems = res.total;
    });
  }

  abrirFormulario() {
    this.formularioVisible = true;
    this.usuarioEditando = null;
  }

  ocultarFormulario() {
    this.formularioVisible = false;
  }

  refrescarUsuarios() {
    this.cargarUsuarios();
    this.estudianteService.obtenerEstudiantes().subscribe(e => this.estudiantes = e);
    this.profesorService.obtenerDocentes().subscribe(d => this.docentes = d);
    this.formularioVisible = false;
    this.usuarioEditando = null;
  }

  editar(usuario: any) {
      const estudiante = this.estudiantes.find(e => e.id === usuario.estudianteId);
      const profesor = this.docentes.find(p => p.id === usuario.profesorId);

      this.usuarioEditando = {
        ...usuario,
        estudianteId: usuario.estudianteId ?? estudiante?.id,
        profesorId: usuario.profesorId ?? profesor?.id
      };

      this.formularioVisible = true;
  }

  eliminar(usuario: any) {
    Swal.fire({
      title: '¿Estás seguro?',
      text: `Estás a punto de eliminar al usuario ${this.getNombreUsuario(usuario)}.`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#d33',
      cancelButtonColor: '#3085d6',
      confirmButtonText: 'Sí, eliminar',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed) {
        this.usuarioService.eliminar(usuario.id).subscribe({
          next: () => {
            Swal.fire('Eliminado', 'El usuario fue eliminado correctamente.', 'success');
            this.cargarUsuarios(); 
          },
          error: () => {
            Swal.fire('Error', 'Hubo un problema al eliminar el usuario.', 'error');
          }
        });
      }
    });
  }

  getNombreUsuario(item: any): string {
    if (item.rol === 'estudiante') {
      const est = this.estudiantes.find(e => e.id === item.estudianteId);
      return est ? `${est.nombre} ${est.apellidos}` : '(Estudiante no encontrado)';
    }
    if (item.rol === 'profesor') { 
      const prof = this.docentes.find(p => p.id === item.profesorId);  
      return prof ? `${prof.nombre} ${prof.apellidos}` : '(Profesor no encontrado)';
    }
    return '—';
  }

  cambiarPagina(nuevaPagina: number) {
    this.page = nuevaPagina;
    this.cargarUsuarios();
  }

  cambiarPageSize(nuevoTamaño: number) {
    this.pageSize = nuevoTamaño;
    this.page = 1;
    this.cargarUsuarios();
  }

  get totalPaginas(): number[] {
    const total = Math.ceil(this.totalItems / this.pageSize);
    return Array.from({ length: total }, (_, i) => i + 1);
  }
}
