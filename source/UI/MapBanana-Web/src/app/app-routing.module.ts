import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GmComponent } from './gm/gm.component';
import { PlayerComponent } from './player/player.component';

const routes: Routes = [
  { path: "gm", component: GmComponent},
  { path: "player", component: PlayerComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
