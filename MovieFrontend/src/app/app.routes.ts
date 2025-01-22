import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';

export const routes: Routes = [
    {
        component : LoginComponent,
        path : 'login'
    },
    {
        component : RegisterComponent,
        path : 'register'
    },    
    {
        component : LoginComponent,
        path : ''
    }
];
