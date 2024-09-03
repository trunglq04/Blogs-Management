import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { AuthService } from '../../../../../../services/auth.service';
import { jwtDecode } from 'jwt-decode';

interface DecodedToken {
  exp: number;
  // Add other properties if needed
}

export const authGuard: CanActivateFn = (route, state) => {
  const cookieService = inject(CookieService);
  const authService = inject(AuthService);
  const router = inject(Router);

  // Get User info from localStorage
  const user = authService.getUser();

  // Check if the JWT Token exists
  let token = cookieService.get('Authorization');

  if (token && user) {
    token = token.replace('Bearer ', '');
    const decodedToken: DecodedToken = jwtDecode<DecodedToken>(token);
    console.log(decodedToken);
    
    // Check if token has expired
    const expirationDate = decodedToken.exp * 1000;
    const currentDate = new Date().getTime();

    if (expirationDate > currentDate) {
      // Token is still valid due to user is defined
      if (user.roles.includes('Writer')) {
        return true;
      } else {
        // Authorized user
        // Can redirect to Unauthorize Page or just send an alert
        alert('Unauthorized');
        return false;
      }
    }
  }

  // Logout
  authService.logout();
  alert('Expired token, you need to login again!');
  return router.createUrlTree(['/login'], {
    queryParams: { returnUrl: state.url },
  });
};
