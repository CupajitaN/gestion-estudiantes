import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../../core/auth/auth.service';
import { MateriaService } from '../../../../core/services/materia.service';

@Component({
  standalone: false,
  selector: 'app-inicio',
  templateUrl: './inicio.component.html',
  styleUrls: ['./inicio.component.css'],
})
export class InicioComponent implements OnInit {
  cursos: any[] = [];
  cursosFiltrados: any[] = [];

  filtroNombre = '';
  
  page = 1;
  pageSize = 10;
  totalItems = 0;
  pageSizeOptions = [5, 10, 20, 50];

  role: string | null = null;

  constructor(private authService: AuthService, private MateriaService: MateriaService, private router: Router) {}

  ngOnInit(): void {
    this.role = this.authService.getUserRole();
    this.cargarDatos();
  }

  cargarDatos() {
    if (this.role === 'admin') {
      this.MateriaService.obtenerMateriasDeTodosLosEstudiantes(this.page, this.pageSize)
        .subscribe((res) => {
          this.cursos = res.data;
          this.totalItems = res.total;
          this.aplicarFiltros();
        });
    } else {
       this.MateriaService.obtenerMateriasEstudiante(this.page, this.pageSize).subscribe((res) => { 
        this.cursos = res.data;
        this.totalItems = res.total;
        this.aplicarFiltros();
      });
    }
  }

  aplicarFiltros() {
    this.cursosFiltrados = this.cursos.filter((curso) => {
      const coincideNombre = this.filtroNombre === '' || curso.nombre.toLowerCase().includes(this.filtroNombre.toLowerCase());
      return coincideNombre;
    });
  }

  resetFiltros() {
    this.filtroNombre = '';
    this.aplicarFiltros();
  }

  cambiarPagina(nuevaPagina: number) {
    this.page = nuevaPagina;
    this.cargarDatos();
  }

  cambiarPageSize(nuevoTamaño: number) {
    this.pageSize = nuevoTamaño;
    this.page = 1;
    this.cargarDatos();
  }

  get totalPaginas(): number[] {
    const total = Math.ceil(this.totalItems / this.pageSize);
    return Array.from({ length: total }, (_, i) => i + 1);
  }

  verAlumnos(materiaId: number): void {
    this.router.navigate(['/dashboard/otros-estudiantes', materiaId]);
  }
}