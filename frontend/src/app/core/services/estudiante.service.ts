import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class EstudianteService {
  private baseUrl = 'http://localhost:5085/api/estudiante';

  constructor(private http: HttpClient) {}

  inscribirse(materiaIds: number[]): Observable<any> {
    return this.http.post(`${this.baseUrl}/inscribirse`, { materiaIds });
  }

  obtenerEstudiantes(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}`);
  }

  crearEstudiante(estudiante: any): Observable<any> {
    return this.http.post(`${this.baseUrl}`, estudiante);
  }

  actualizarEstudiante(id: number, estudiante: any) {
    return this.http.put(`${this.baseUrl}/${id}`, estudiante);
  }

  obtenerCreditosEstudiante(): Observable<{ maximos: number; utilizados: number }> {
    return this.http.get<{ maximos: number; utilizados: number }>(`${this.baseUrl}/creditos`);
  }

  obtenerEstudiantesPorMateria(materiaId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/por-materia/${materiaId}`);
  }

  obtenerEstudiantesPorMateriaDetalle(materiaId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/por-materia-detalle/${materiaId}`);
  }

}
