import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EstudianteService } from '../../../../core/services/estudiante.service';

@Component({
  standalone: false,
  selector: 'app-ver-otros-estudiantes',
  templateUrl: './ver-otros-estudiantes.component.html'
})
export class VerOtrosEstudiantesComponent implements OnInit {
  estudiantes: any[] = [];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private estudianteService: EstudianteService
  ) {}

  ngOnInit(): void {
    const materiaId = Number(this.route.snapshot.paramMap.get('materiaId'));
    this.estudianteService.obtenerEstudiantesPorMateriaDetalle(materiaId).subscribe({
      next: (data) => this.estudiantes = data,
      error: (err) => console.error('Error al obtener estudiantes', err)
    });
  }

  volver() {
    this.router.navigate(['/dashboard/inicio']);
  }
}