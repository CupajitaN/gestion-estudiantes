import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ProfesorService {
  private baseUrl = 'http://localhost:5085/api/profesor';

  constructor(private http: HttpClient) {}

    obtenerDocentes(): Observable<any[]> {
        return this.http.get<any[]>(`${this.baseUrl}`);
    }

    crearProfesor(profesor: any): Observable<any> {
        return this.http.post(`${this.baseUrl}`, profesor);
    }

    asignarMateriaAProfesor(profesorId: number, materiaId: number): Observable<any> {
        return this.http.post(`${this.baseUrl}/asignar?profesorId=${profesorId}&materiaId=${materiaId}`, {});
    }

    actualizarProfesor(id: number, profesor: any) {
        return this.http.put(`${this.baseUrl}/${id}`, profesor);
    }
}
