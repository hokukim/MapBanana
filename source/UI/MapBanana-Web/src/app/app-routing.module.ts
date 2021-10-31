import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CampaignComponent } from './campaign/campaign.component';
import { GlComponent } from './gl/gl.component';
import { PlayerComponent } from './player/player.component';

const routes: Routes = [
  { path: "gl", component: GlComponent},
  { path: "player", component: PlayerComponent},
  { path: "campaign", component: CampaignComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
