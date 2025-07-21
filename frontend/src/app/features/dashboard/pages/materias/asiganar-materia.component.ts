
import { Component, OnInit } from '@angular/core';
import { MateriaService } from '../../../../core/services/materia.service';
import { ProfesorService } from '../../../../core/services/profesor.service';
import Swal from 'sweetalert2';

@Component({
  standalone: false,
  selector: 'app-asignar-materia',
  templateUrl: './asiganar-materia.component.html'
})
export class AsignarMateriaComponent implements OnInit {
  profesores: any[] = [];
  materias: any[] = [];

  materiaId: number | null = null;
  profesorId: number | null = null;

  constructor(
    private materiaService: MateriaService,
    private profesorService: ProfesorService
  ) {}

  ngOnInit(): void {
    this.cargarMaterias();
    this.cargarProfesores();
  }

  cargarMaterias() {
    this.materiaService.obtenerTodas().subscribe({
      next: res => this.materias = res.data,
      error: () => Swal.fire('Error', 'No se pudieron cargar las materias.', 'error')
    });
  }

  cargarProfesores() {
    this.profesorService.obtenerDocentes().subscribe({
      next: data => this.profesores = data,
      error: () => Swal.fire('Error', 'No se pudieron cargar los profesores.', 'error')
    });
  }

  asignarMateria() {
    if (this.profesorId == null || this.materiaId == null) {
      Swal.fire('Campos requeridos', 'Selecciona un profesor y una materia.', 'warning');
      return;
    }

    this.materiaService.asignarMateria(this.profesorId, this.materiaId).subscribe({
      next: () => {
        Swal.fire('Asignación exitosa', 'La materia fue asignada al profesor correctamente.', 'success');
        this.materiaId = null;
        this.profesorId = null;
      },
      error: (error) => {
        const mensaje = error?.error?.error || 'Error al asignar materia.';
        Swal.fire('Error', mensaje, 'error');
      }
    });
  }
}
