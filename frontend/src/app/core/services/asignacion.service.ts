import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AsignacionService {
    private baseUrl = 'http://localhost:5085/api/asignacion';

    constructor(private http: HttpClient) {}

    obtenerTodas(page?: number, pageSize?: number): Observable<{ data: any[], total: number }> {
        let params: any = {};
        if (page !== undefined && pageSize !== undefined) {
            params.page = page;
            params.pageSize = pageSize;
        }

        return this.http.get<{ data: any[], total: number }>(`${this.baseUrl}/todas`, { params });
    }

    obtenerPorProfesor(profesorId: number): Observable<any[]> {
        return this.http.get<any[]>(`${this.baseUrl}/profesor/${profesorId}`);
    }

   asignarMateria(profesorId: number, materiaId: number): Observable<any> {
    return this.http.post(`${this.baseUrl}/asignar?profesorId=${profesorId}&materiaId=${materiaId}`, {});
    }

    actualizarAsignacion(id: number, profesorId: number, materiaId: number): Observable<any> {
        return this.http.put(`${this.baseUrl}/asignaciones/${id}`, { profesorId, materiaId });
    }

    eliminarAsignacion(id: number): Observable<any> {
        return this.http.delete(`${this.baseUrl}/asignaciones/${id}`);
    }
}
