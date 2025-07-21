import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class UsuarioService {
  private baseUrl = 'http://localhost:5085/api/usuario';

  constructor(private http: HttpClient) {}

  crear(usuario: any): Observable<any> {
    return this.http.post(this.baseUrl, usuario);
  }

  obtenerPorId(id: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/${id}`);
  }

  obtenerTodos(page?: number, pageSize?: number): Observable<{ data: any[], total: number }> {
    const params: any = {};
    if (page !== undefined && pageSize !== undefined) {
      params.page = page;
      params.pageSize = pageSize;
    }

    return this.http.get<{ data: any[], total: number }>(this.baseUrl, { params });
  }

  eliminar(id: number) {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }

  obtenerProfesorIdPorUsuario(usuarioId: number) {
    return this.http.get<{ profesorId: number }>(`${this.baseUrl}/${usuarioId}/profesor-id`);
  }

}
