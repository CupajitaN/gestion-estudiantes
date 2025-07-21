import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { MateriaService } from '../../../../core/services/materia.service';
import { EstudianteService } from '../../../../core/services/estudiante.service';
import { AsignacionService } from '../../../../core/services/asignacion.service';
import { ProfesorService } from '../../../../core/services/profesor.service';
import Swal from 'sweetalert2';

@Component({
  standalone: false,
  selector: 'app-inscribir-materias',
  templateUrl: './inscribir-materias.component.html',
})
export class InscribirMateriasComponent implements OnInit {
    @Output() materiasInscritas = new EventEmitter<void>();
    @Output() cancelar = new EventEmitter<void>();

    materiasDisponibles: any[] = [];
    materiasSeleccionadas: number[] = [];

    creditosMaximos: number = 0;
    creditosUtilizados: number = 0;
    creditosEnProceso: number = 0;

    profesores: any[] = [];
    asignaciones: any[] = [];

    constructor(
        private materiaService: MateriaService,
        private estudianteService: EstudianteService,
        private asignacionService: AsignacionService,
        private profesorService: ProfesorService
    ) {}

    ngOnInit(): void {
        this.materiaService.obtenerTodas().subscribe({
            next: (res) => this.materiasDisponibles = res.data,
            error: (err) => console.error('Error al obtener materias disponibles:', err)
        });

        this.estudianteService.obtenerCreditosEstudiante().subscribe({
            next: (res) => {
            this.creditosMaximos = res.maximos;
            this.creditosUtilizados = res.utilizados;
            },
            error: (err) => console.error('Error al obtener créditos:', err)
        });

        this.asignacionService.obtenerTodas().subscribe({
            next: (res) => this.asignaciones = res.data,
            error: (err) => console.error('Error al obtener asignaciones:', err)
        });

        this.profesorService.obtenerDocentes().subscribe({
            next: (res) => this.profesores = res,
            error: (err) => console.error('Error al obtener profesores:', err)
        });
    }

    toggleSeleccion(id: number): void {
        if (this.materiasSeleccionadas.includes(id)) {
            this.materiasSeleccionadas = this.materiasSeleccionadas.filter(m => m !== id);
            this.creditosEnProceso -= 3;
        } else {
            this.materiasSeleccionadas.push(id);
            this.creditosEnProceso += 3;
        }
    }

    confirmarInscripcion(): void {
        if (this.materiasSeleccionadas.length !== 3) {
        Swal.fire('Error', 'Debes seleccionar exactamente 3 materias con profesores diferentes.', 'error');
        return;
        }

        this.estudianteService.inscribirse(this.materiasSeleccionadas).subscribe({
        next: () => {
            Swal.fire('Éxito', 'Materias inscritas correctamente.', 'success');
            this.materiasInscritas.emit();
        },
        error: (err) => {
            Swal.fire('Error', err.error?.mensaje || 'Error al inscribirse', 'error');
        },
        });
    }

    cancelarFormulario(): void {
        this.cancelar.emit();
    }

    obtenerNombreProfesor(materiaId: number): string {
        const asignacion = this.asignaciones.find(a => a.materiaId === materiaId);
        if (!asignacion) return 'Sin asignar';

        const profesor = this.profesores.find(p => p.id === asignacion.profesorId);
        return profesor ? `${profesor.nombre} ${profesor.apellidos}` : 'Sin asignar';
    }
}
