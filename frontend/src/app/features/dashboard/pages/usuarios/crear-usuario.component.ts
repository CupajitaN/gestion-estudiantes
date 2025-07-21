import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { UsuarioService } from '../../../../core/services/usuario.service';
import { EstudianteService } from '../../../../core/services/estudiante.service';
import { ProfesorService } from '../../../../core/services/profesor.service';
import Swal from 'sweetalert2';


@Component({
  standalone: false,
  selector: 'app-crear-usuario',
  templateUrl: './crear-usuario.component.html',
})
export class CrearUsuarioComponent implements OnInit {
    @Input() usuario: any = null;
    @Output() usuarioCreado = new EventEmitter<void>();
    @Output() cancelar = new EventEmitter<void>();

    nuevoUsuario = {
        correo: '',
        contrasena: '',
        rol: '',
        nombres: '',
        apellidos: '',
        jornada: '',
        programa: '', 
    };
    estudiantes: any[] = [];
    docentes: any[] = [];
    verPassword: boolean = false;

    constructor(
        private usuarioService: UsuarioService,
        private estudianteService: EstudianteService,
        private profesorService: ProfesorService
    ) {}

    ngOnInit(): void {
        this.estudianteService.obtenerEstudiantes().subscribe(e => {
            this.estudiantes = e;

            if (this.usuario?.rol === 'estudiante' && this.usuario.estudiante) {
            const est = this.usuario.estudiante;
            this.nuevoUsuario.nombres = est.nombre;
            this.nuevoUsuario.apellidos = est.apellidos;
            this.nuevoUsuario.jornada = est.jornada;
            this.nuevoUsuario.programa = est.programa;
            }
        });

        this.profesorService.obtenerDocentes().subscribe(d => {
            this.docentes = d;

            if (this.usuario?.rol === 'profesor' && this.usuario.profesor) {
            const prof = this.usuario.profesor;
            this.nuevoUsuario.nombres = prof.nombre;
            this.nuevoUsuario.apellidos = prof.apellidos;
            this.nuevoUsuario.jornada = prof.jornada;
            }
        });

        if (this.usuario) {
            this.nuevoUsuario.correo = this.usuario.correo;
            this.nuevoUsuario.rol = this.usuario.rol;
        }
    }

    guardarUsuario() {
    if (this.usuario) {
        this.actualizarUsuario();
    } else {
        this.crearNuevoUsuario();
    }
    }

    crearNuevoUsuario() {
        const { rol, nombres, apellidos, correo, contrasena, jornada, programa } = this.nuevoUsuario;

        if (rol === 'estudiante') {
            this.estudianteService.crearEstudiante({
            nombre: nombres,
            apellidos,
            correo,
            programa,
            jornada,
            creditosMaximos: 9,
            creditosUtilizados: 0
            }).subscribe(estudiante => {
            this.crearUsuarioFinal(correo, contrasena, rol, estudiante.id, null);
            });
        } else if (rol === 'docente') {
            this.profesorService.crearProfesor({
            nombre: nombres,
            apellidos,
            correo,
            jornada,
            materiasMax: 2
            }).subscribe(profesor => {
            this.crearUsuarioFinal(correo, contrasena, rol, null, profesor.id);
            });
        } else {
            this.crearUsuarioFinal(correo, contrasena, rol, null, null);
        }
    }

    actualizarUsuario() {
        const { rol, nombres, apellidos, correo, jornada, programa } = this.nuevoUsuario;

        if (rol === 'estudiante') {
            this.estudianteService.actualizarEstudiante(this.usuario.estudianteId, {
            nombre: nombres,
            apellidos,
            jornada,
            programa
            }).subscribe(() => {
            this.finalizarEdicion();
            });
        } else if (rol === 'docente') {
            this.profesorService.actualizarProfesor(this.usuario.profesorId, {
            nombre: nombres,
            apellidos,
            jornada
            }).subscribe(() => {
            this.finalizarEdicion();
            });
        } else {
            this.finalizarEdicion();
        }
    }

    cancelarFormulario() {
        this.cancelar.emit();
    }

    onRolChange() {
        this.nuevoUsuario.nombres = '';
        this.nuevoUsuario.apellidos = '';
    }

    crearUsuarioFinal(correo: string, contrasenaHash: string, rol: string, estudianteId: number | null, profesorId: number | null) {
        const usuario = {
            correo,
            contrasenaHash,
            rol,
            estudianteId,
            profesorId,
        };

        this.usuarioService.crear(usuario).subscribe({
            next: () => {
            Swal.fire('Éxito', 'Usuario creado correctamente', 'success');
            this.usuarioCreado.emit();
            },
            error: (err) => {
            Swal.fire('Error', 'Hubo un error al crear el usuario', 'error');
            console.error(err);
            }
        });
    }

    finalizarEdicion() {
        Swal.fire('Actualizado', 'Usuario actualizado correctamente', 'success');
        this.usuarioCreado.emit();
    }
}
