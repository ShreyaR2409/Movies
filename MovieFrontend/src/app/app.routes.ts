import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { AuthGuard } from './guard/auth.guard';
import { NotfoundComponent } from './components/notfound/notfound.component';
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
        component: LoginComponent,
        path: ''
    },
    {
        component: NavbarComponent,
        path: '',
        canActivate: [AuthGuard],
        children: [
            {
                component: DashboardComponent,
                path: 'dashboard'
            }
        ]
    },
    {
        path: '**',
        component : NotfoundComponent
    }
];
