import { Component, OnInit } from '@angular/core';
import { MateriaService } from '../../../../core/services/materia.service';
import Swal from 'sweetalert2';

@Component({
  standalone: false,
  selector: 'app-materias',
  templateUrl: './materias.component.html',
})
export class MateriasComponent implements OnInit {
  materias: any[] = [];
  formularioVisible = false;
  materiaEditando: any = null;

  page = 1;
  pageSize = 10;
  totalItems = 0;
  pageSizeOptions = [5, 10, 20, 50];

  constructor(private materiaService: MateriaService) {}

  ngOnInit(): void {
    this.cargarMaterias();
  }

  cargarMaterias() {
    this.materiaService.obtenerTodas(this.page, this.pageSize).subscribe((res) => {
      this.materias = res.data;
      this.totalItems = res.total;
    });
  }

  cambiarPagina(nuevaPagina: number) {
    this.page = nuevaPagina;
    this.cargarMaterias();
  }

  cambiarPageSize(nuevoTamaño: number) {
    this.pageSize = nuevoTamaño;
    this.page = 1;
    this.cargarMaterias();
  }

  get totalPaginas(): number[] {
    const total = Math.ceil(this.totalItems / this.pageSize);
    return Array.from({ length: total }, (_, i) => i + 1);
  }
  
  abrirFormulario() {
    this.formularioVisible = true;
    this.materiaEditando = null;
  }

  ocultarFormulario() {
    this.formularioVisible = false;
  }

  refrescarMaterias() {
    this.cargarMaterias();
    this.formularioVisible = false;
    this.materiaEditando = null;
  }

  editar(materia: any) {
    this.materiaEditando = { ...materia };
    this.formularioVisible = true;
  }

  eliminar(materia: any) {
    Swal.fire({
      title: '¿Estás seguro?',
      text: `Estás a punto de eliminar la materia "${materia.nombre}".`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#d33',
      cancelButtonColor: '#3085d6',
      confirmButtonText: 'Sí, eliminar',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed) {
        this.materiaService.eliminar(materia.id).subscribe({
          next: () => {
            Swal.fire('Eliminado', 'La materia fue eliminada correctamente.', 'success');
            this.cargarMaterias();
          },
          error: () => {
            Swal.fire('Error', 'Hubo un problema al eliminar la materia.', 'error');
          }
        });
      }
    });
  }
}
