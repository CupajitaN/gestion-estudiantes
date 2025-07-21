import { Component, OnInit } from '@angular/core';
import { MateriaService } from '../../../../core/services/materia.service';
import { Router } from '@angular/router';

@Component({
  standalone: false,
  selector: 'app-ver-materias-estudiante',
  templateUrl: './ver-materias-estudiante.component.html',
})
export class VerMateriasEstudianteComponent implements OnInit {
  materias: any[] = [];
  formularioVisible: boolean = false;

  page = 1;
  pageSize = 10;
  totalItems = 0;
  pageSizeOptions = [5, 10, 20, 50];

  constructor(
    private materiaService: MateriaService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.cargarMaterias();
  }

  cargarMaterias(): void {
    this.materiaService.obtenerMateriasEstudiante(this.page, this.pageSize).subscribe({
      next: (res) => {
        this.materias = res.data;
        this.totalItems = res.total;
      },
      error: (err) => console.error('Error al cargar materias del estudiante:', err)
    });
  }

  cambiarPagina(nuevaPagina: number): void {
    this.page = nuevaPagina;
    this.cargarMaterias();
  }

  cambiarPageSize(nuevoTamaño: number): void {
    this.pageSize = nuevoTamaño;
    this.page = 1;
    this.cargarMaterias();
  }

  get totalPaginas(): number[] {
    const total = Math.ceil(this.totalItems / this.pageSize);
    return Array.from({ length: total }, (_, i) => i + 1);
  }

  abrirFormulario(): void {
    this.formularioVisible = true;
  }

  ocultarFormulario(): void {
    this.formularioVisible = false;
  }

  actualizarListado(): void {
    this.cargarMaterias();
    this.ocultarFormulario();
  }
}
