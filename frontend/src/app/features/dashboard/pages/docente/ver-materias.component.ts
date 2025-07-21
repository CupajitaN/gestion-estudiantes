import { Component, OnInit } from '@angular/core';
import { MateriaService } from '../../../../core/services/materia.service';
import { AuthService } from '../../../../core/auth/auth.service';
import { UsuarioService } from '../../../../core/services/usuario.service';
import { Router } from '@angular/router';

@Component({
  standalone: false,
  selector: 'app-ver-materias',
  templateUrl: './ver-materias.component.html',
})
export class VerMateriasComponent implements OnInit {
  materias: any[] = [];

  page = 1;
  pageSize = 5;
  totalItems = 0;
  pageSizeOptions = [5, 10, 20];

  constructor(
    private materiaService: MateriaService,
    private usuarioService: UsuarioService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.cargarMateriasAsignadas();
  }

  cargarMateriasAsignadas(): void {
    const usuario = this.authService.obtenerUsuarioActual();
    if (usuario?.id) {
      this.usuarioService.obtenerProfesorIdPorUsuario(usuario.id).subscribe({
        next: (res) => {
          const profesorId = res.profesorId;
          this.materiaService.obtenerMateriasPorProfesor(profesorId, this.page, this.pageSize).subscribe({
            next: (res) => {
              this.materias = res.data;
              this.totalItems = res.total;
            },
            error: (err) => console.error('Error al obtener materias:', err)
          });
        },
        error: (err) => console.error('No se pudo obtener el profesor:', err)
      });
    }
  }

  cambiarPagina(nuevaPagina: number): void {
    this.page = nuevaPagina;
    this.cargarMateriasAsignadas();
  }

  cambiarPageSize(nuevoTamaño: number): void {
    this.pageSize = nuevoTamaño;
    this.page = 1;
    this.cargarMateriasAsignadas();
  }

  get totalPaginas(): number[] {
    const total = Math.ceil(this.totalItems / this.pageSize);
    return Array.from({ length: total }, (_, i) => i + 1);
  }

  verAlumnos(materiaId: number): void {
    this.router.navigate(['/dashboard/ver-alumnos', materiaId]);
  }
}
