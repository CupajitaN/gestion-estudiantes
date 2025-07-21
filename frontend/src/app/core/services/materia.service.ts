import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class MateriaService {
  private baseUrl = 'http://localhost:5085/api/materia';

  constructor(private http: HttpClient) {}

  obtenerTodas(page?: number, pageSize?: number): Observable<{ data: any[], total: number }> {
    let options = {};

    if (page !== undefined && pageSize !== undefined) {
      options = {
        params: {
          page: page.toString(),
          pageSize: pageSize.toString()
        }
      };
    }

    return this.http.get<{ data: any[], total: number }>(this.baseUrl, options);
  }

  obtenerPorId(id: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/${id}`);
  }

  crear(materia: any): Observable<any> {
    return this.http.post(this.baseUrl, materia);
  }

  actualizar(id: number, materia: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/${id}`, materia);
  }

  eliminar(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }

  asignarMateria(profesorId: number, materiaId: number) {
    return this.http.post(`${this.baseUrl}/asignar`, null, {
      params: {
        profesorId,
        materiaId
      }
    });
  }

  obtenerMateriasPorProfesor(profesorId: number, page: number, pageSize: number): Observable<{ data: any[], total: number }> {
    return this.http.get<{ data: any[], total: number }>(`${this.baseUrl}/profesor/${profesorId}`, {
      params: {
        page: page,
        pageSize: pageSize
      }
    });
  }

  obtenerMateriasEstudiante(page: number, pageSize: number): Observable<{ data: any[], total: number }> {
    return this.http.get<{ data: any[], total: number }>(`${this.baseUrl}/estudiante`, {
      params: {
        page,
        pageSize
      }
    });
  }

  obtenerMateriasDeTodosLosEstudiantes(page: number, pageSize: number): Observable<{ data: any[], total: number }> {
  return this.http.get<{ data: any[], total: number }>(`${this.baseUrl}/estudiantes`, {
    params: {
      page,
      pageSize
    }
  });
}
  
}
