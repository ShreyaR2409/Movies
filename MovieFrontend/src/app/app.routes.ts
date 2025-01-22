import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { NavbarComponent } from './components/navbar/navbar.component';

export const routes: Routes = [
    {
        component: LoginComponent,
        path: 'login'
    },
    {
        component: RegisterComponent,
        path: 'register'
    },
    {
        component: NavbarComponent,
        path: '',
        children: [
            {
                component: DashboardComponent,
                path: 'dashboard'
            }
        ]
    },
    {
        component: LoginComponent,
        path: ''
    }
];
