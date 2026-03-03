import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { BacklogItem } from '../models';

@Injectable({ providedIn: 'root' })
export class BacklogService {
  private api = environment.apiUrl;
  constructor(private http: HttpClient) {}

  getAll() { return this.http.get<BacklogItem[]>(`${this.api}/backlog`); }
  create(item: BacklogItem) { return this.http.post<BacklogItem>(`${this.api}/backlog`, item); }
  update(id: number, item: BacklogItem) { return this.http.put<BacklogItem>(`${this.api}/backlog/${id}`, item); }
  delete(id: number) { return this.http.delete(`${this.api}/backlog/${id}`); }
}
