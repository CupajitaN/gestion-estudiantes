import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { EstudianteService } from '../../../../core/services/estudiante.service';

@Component({
  standalone: false,
  selector: 'app-ver-alumnos',
  templateUrl: './ver-alumnos.component.html',
})
export class VerAlumnosComponent implements OnInit {
  estudiantes: any[] = [];
  materiaId!: number;

  constructor(
    private route: ActivatedRoute,
    private estudianteService: EstudianteService,
    private location: Location
  ) {}

  ngOnInit(): void {
    this.materiaId = +this.route.snapshot.paramMap.get('materiaId')!;
    this.cargarEstudiantes();
  }

  cargarEstudiantes(): void {
    this.estudianteService.obtenerEstudiantesPorMateria(this.materiaId).subscribe({
      next: (res) => this.estudiantes = res,
      error: (err) => console.error('Error al cargar estudiantes:', err)
    });
  }

  volver(): void {
    this.location.back();
    }
}
