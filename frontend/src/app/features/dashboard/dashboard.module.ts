import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms'
import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardComponent } from './dashboard.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
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

@NgModule({
  declarations: [
    DashboardComponent,
    SidebarComponent,
    InicioComponent,
    UsuariosComponent,
    CrearUsuarioComponent,
    MateriasComponent,
    CrearMateriaComponent,
    AsignarMateriaComponent,
    MateriasAsignadasComponent,
    VerMateriasComponent,
    VerMateriasEstudianteComponent,
    InscribirMateriasComponent,
    VerAlumnosComponent,
    VerOtrosEstudiantesComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    DashboardRoutingModule,
    ReactiveFormsModule
  ]
})
export class DashboardModule {}
