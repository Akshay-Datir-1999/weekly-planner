import { Injectable, inject } from '@angular/core';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class BacklogService {

  private api = inject(ApiService);

  getAll() {
    return this.api.get<any[]>('backlog');
  }
}