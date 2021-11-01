import { Component, OnInit, ChangeDetectionStrategy, Input, Output } from '@angular/core';
import { Store } from '@ngrx/store';
import { BananaApiService } from '../services/banana-api.service';
import { selectCampaigns } from '../store/campaigns/campaigns.selector';
import { IAppState } from '../app.state';

@Component({
  selector: 'app-gl',
  templateUrl: './gl.component.html',
  styleUrls: ['./gl.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class GlComponent implements OnInit {
  public campaigns$ = this.store.select(selectCampaigns);

  constructor(
    private store: Store<IAppState>,
    private bananaApiService: BananaApiService) {
  }

  ngOnInit(): void {
    this.bananaApiService.getCampaigns();
  }
}