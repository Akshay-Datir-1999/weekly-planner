import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Member } from '../models';

@Injectable({ providedIn: 'root' })
export class RoleService {
  private members: Member[] = [
    { id: 1, name: 'Team Lead', isLead: true },
    { id: 2, name: 'Alice', isLead: false },
    { id: 3, name: 'Bob', isLead: false },
  ];
  private current = new BehaviorSubject<Member>(this.members[0]);
  currentMember$ = this.current.asObservable();

  getMembers(): Member[] { return this.members; }
  getCurrent(): Member { return this.current.value; }
  switchTo(m: Member): void { this.current.next(m); }
  isLead(): boolean { return this.current.value.isLead; }
}
