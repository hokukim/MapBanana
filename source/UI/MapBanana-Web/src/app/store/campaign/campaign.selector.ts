import { IAppState, ICampaignsState } from 'src/app/app.store';
import { ICampaign } from './campaign.model';

import { createSelector } from '@ngrx/store';
 
export const selectCampaigns = (state: ICampaignsState) => state.campaigns;
 
export const selectCampaign = (id: string) => createSelector(
  selectCampaigns,
  (campaigns: Map<string, ICampaign>) => campaigns.get(id)
);