import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../../../core/auth/auth.service';
import { AsignacionService } from '../../../../core/services/asignacion.service';
import { ProfesorService } from '../../../../core/services/profesor.service';
import { MateriaService } from '../../../../core/services/materia.service';
import Swal from 'sweetalert2';

@Component({
  standalone: false,
  selector: 'app-materias-asignadas',
  templateUrl: './materias-asignadas.component.html',
})
export class MateriasAsignadasComponent implements OnInit {
    materiasAsignadas: any[] = [];
    esAdmin = false;
    formulario!: FormGroup;
    profesores: any[] = [];
    materias: any[] = [];
    asignaciones: any[] = [];
    mostrarFormulario = false;
    modoEdicion: boolean = false;
    asignacionIdEditando: number | null = null;

    page = 1;
    pageSize = 10;
    totalItems = 0;
    pageSizeOptions = [5, 10, 20, 50];

    constructor(
        private authService: AuthService,
        private fb: FormBuilder,
        private profesorService: ProfesorService,
        private materiaService: MateriaService,
        private asignacionService: AsignacionService
    ) {}

    ngOnInit(): void {
        this.formulario = this.fb.group({
        profesorId: ['', Validators.required],
        materiaId: ['', Validators.required],
        });

        const user = this.authService.obtenerUsuarioActual();
        this.esAdmin = user?.rol === 'admin';

       if (this.esAdmin) {
            this.asignacionService.obtenerTodas(this.page, this.pageSize).subscribe(res => {
                this.materiasAsignadas = res.data.map((asig: any) => ({
                materiaNombre: asig.nombreMateria,
                profesorNombre: asig.nombreProfesor
                }));
                this.totalItems = res.total;
            });
        } else {
        this.asignacionService.obtenerPorProfesor(user.id).subscribe(data => {
            this.materiasAsignadas = data.map((asig: any) => ({
            materiaNombre: asig.nombreMateria,
            profesorNombre: asig.nombreProfesor
            }));
        });
        }

        this.profesorService.obtenerDocentes().subscribe(data => this.profesores = data);
        this.materiaService.obtenerTodas().subscribe(res => {
            this.materias = res.data;
            this.totalItems = res.total;
        });
        this.cargarAsignaciones();
    }

    cargarAsignaciones() {
        this.asignacionService.obtenerTodas(this.page, this.pageSize).subscribe(res => {
            this.asignaciones = res.data.map((asig: any) => {
            const profesor = this.profesores.find(p => p.nombre === asig.nombreProfesor);
            const nombreCompleto = profesor
                ? `${profesor.nombre} ${profesor.apellidos}`
                : asig.nombreProfesor;

            const profesorId = profesor?.id || asig.profesorId;
            const materia = this.materias.find(m => m.nombre === asig.nombreMateria);
            const materiaId = materia?.id || asig.materiaId;

            return {
                id: asig.id,
                profesorId,
                materiaId,
                profesorNombre: nombreCompleto,
                materiaNombre: asig.nombreMateria
            };
            });
            this.totalItems = res.total;
        });
    }


    asignarMateria() {
        const { profesorId, materiaId } = this.formulario.value;

        if (this.modoEdicion && this.asignacionIdEditando !== null) {
            // Modo edición
            this.asignacionService.actualizarAsignacion(this.asignacionIdEditando, profesorId, materiaId).subscribe(() => {
            Swal.fire('Actualizado', 'Asignación actualizada correctamente.', 'success');
            this.resetFormulario();
            }, (error) => {
            const mensaje = error?.error?.error || 'Ocurrió un error al actualizar.';
            Swal.fire('Error', mensaje, 'error');
            });

        } else {
            // Modo nuevo
            this.asignacionService.asignarMateria(profesorId, materiaId).subscribe(() => {
            Swal.fire('Éxito', 'Materia asignada correctamente.', 'success');
            this.resetFormulario();
            }, (error) => {
            const mensaje = error?.error?.error || 'Ocurrió un error al asignar la materia.';
            Swal.fire('Error', mensaje, 'error');
            });
        }
    }

    editar(asignacion: any) {
        this.modoEdicion = true;
        this.asignacionIdEditando = asignacion.id;

        this.formulario.patchValue({
            profesorId: asignacion.profesorId,
            materiaId: asignacion.materiaId
        });

        this.mostrarFormulario = true;
    }

    eliminar(asignacion: any) {
        Swal.fire({
            title: '¿Estás seguro?',
            text: `¿Deseas eliminar la asignación de ${asignacion.materiaNombre} al profesor ${asignacion.profesorNombre}?`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Sí, eliminar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
            this.asignacionService.eliminarAsignacion(asignacion.id).subscribe(() => {
                Swal.fire('Eliminado', 'La asignación fue eliminada.', 'success');
                this.cargarAsignaciones();
            }, (error) => {
                const mensaje = error?.error?.error || 'Ocurrió un error al eliminar.';
                Swal.fire('Error', mensaje, 'error');
            });
            }
        });
    }

    resetFormulario() {
        this.formulario.reset({
            profesorId: '',
            materiaId: ''
        });
        this.mostrarFormulario = false;
        this.modoEdicion = false;
        this.asignacionIdEditando = null;
        this.cargarAsignaciones();
    }

    alternarFormulario() {
        if (this.mostrarFormulario && this.modoEdicion) {
            this.resetFormulario();
        } else {
            this.mostrarFormulario = !this.mostrarFormulario;
        }
    }

    cambiarPagina(nuevaPagina: number) {
        this.page = nuevaPagina;
        this.cargarAsignaciones();
    }

    cambiarPageSize(nuevoTamaño: number) {
        this.pageSize = nuevoTamaño;
        this.page = 1;
        this.cargarAsignaciones();
    }

    get totalPaginas(): number[] {
        const total = Math.ceil(this.totalItems / this.pageSize);
        return Array.from({ length: total }, (_, i) => i + 1);
    }
}
