import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './dashboard.component';
import { AuthGuard } from '../../core/auth/auth.guard';
import { InicioComponent } from './pages/inicio/inicio.component';
import { UsuariosComponent } from './pages/usuarios/usuarios.component';
import { CrearUsuarioComponent } from './pages/usuarios/crear-usuario.component';
import { MateriasComponent } from './pages/materias/materias.component';
import { CrearMateriaComponent } from './pages/materias/crear-materia.component';
import { AsignarMateriaComponent } from './pages/materias/asiganar-materia.component';
import { MateriasAsignadasComponent } from './pages/materias/materias-asignadas.component';
import { VerMateriasComponent } from './pages/docente/ver-materias.component';
import { VerMateriasEstudianteComponent } from './pages/estudiante/ver-materias-estudiante.component';
import { InscribirMateriasComponent } from './pages/estudiante/inscribir-materias.component';
import { VerAlumnosComponent } from './pages/docente/ver-alumnos.component';
import { VerOtrosEstudiantesComponent } from './pages/estudiante/ver-otros-estudiantes.component';

const routes: Routes = [
  {
    path: '',
    component: DashboardComponent,
    canActivate: [AuthGuard],
    children: [
      { path: 'inicio', component: InicioComponent },
      { path: 'usuarios', component: UsuariosComponent },
      { path: 'crear-usuarios', component: CrearUsuarioComponent },
      { path: 'materias', component: MateriasComponent },
      { path: 'crear-materias', component: CrearMateriaComponent },
      { path: 'asignar-materia', component: AsignarMateriaComponent },
      { path: 'materias-asignadas', component: MateriasAsignadasComponent },
      { path: 'ver-materias', component: VerMateriasComponent },
      { path: 'mis-materias', component: VerMateriasEstudianteComponent },
      { path: 'inscribir-materias', component: InscribirMateriasComponent },
      { path: 'ver-alumnos/:materiaId', component: VerAlumnosComponent },
      { path: 'otros-estudiantes/:materiaId', component: VerOtrosEstudiantesComponent },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DashboardRoutingModule {}
