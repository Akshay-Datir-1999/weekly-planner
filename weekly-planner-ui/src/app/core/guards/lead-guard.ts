import { CanActivateFn } from '@angular/router';

export const leadGuard: CanActivateFn = (route, state) => {
  return true;
};
